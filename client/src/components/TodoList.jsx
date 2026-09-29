import { useMemo } from 'react';
import { AgGridReact } from 'ag-grid-react';

export default function TodoList({
    todos,
    onEdit,
    onDelete,
}) {
    function isTodoOverdue(todo) {
        if (todo.isComplete || !todo.dueDate) {
            return false;
        }

        const dueDate = todo.dueDate.split('T')[0];

        const today = new Date()
            .toLocaleDateString('en-CA');

        return dueDate < today;
    }

    const columnDefs = useMemo(
        () => [
            {
                headerName: '',
                width: 100,
                sortable: false,
                filter: false,
                cellRenderer: (params) => (
                    <button
                        type="button"
                        className="btn btn-priority-dark btn-sm"
                        onClick={() => {
                            params.api.startEditingCell({
                                rowIndex: params.node.rowIndex,
                                colKey: 'title',
                            });
                        }}
                    >
                        Edit
                    </button>
                ),
            },
            {
                field: 'title',
                headerName: 'Todo',
                flex: 2,
                minWidth: 300,
                editable: true,
                cellEditor: 'agTextCellEditor',
                onCellValueChanged: (params) => {
                    if (
                        params.oldValue !==
                        params.newValue
                    ) {
                        onEdit({
                            ...params.data,
                            title: params.newValue,
                        });
                    }
                },
            },
            {
                field: 'dueDate',
                headerName: 'Due Date',
                width: 170,
                sortable: true,
                valueFormatter: (params) =>
                    params.value
                        ? new Date(
                            `${params.value.split('T')[0]}T00:00:00`
                        ).toLocaleDateString()
                        : '',
            },
            {
                field: 'isComplete',
                headerName: 'Status',
                width: 150,
                sortable: true,
                cellRenderer: (params) => {
                    const todo = params.data;

                    if (isTodoOverdue(todo)) {
                        return (
                            <span className="badge text-bg-danger">
                                Overdue
                            </span>
                        );
                    }

                    return (
                        <span
                            className={
                                todo.isComplete
                                    ? 'badge text-bg-success'
                                    : 'badge text-bg-secondary'
                            }
                        >
                            {todo.isComplete
                                ? 'Closed'
                                : 'Open'}
                        </span>
                    );
                },
            },
            {
                field: 'createDate',
                headerName: 'Created',
                width: 170,
                sortable: true,
                valueFormatter: (params) =>
                    params.value
                        ? new Date(
                            params.value
                        ).toLocaleDateString()
                        : '',
            },
            {
                headerName: 'Actions',
                width: 140,
                sortable: false,
                filter: false,
                cellRenderer: (params) => (
                    <button
                        type="button"
                        className="btn btn-danger btn-sm"
                        onClick={() =>
                            onDelete(params.data)
                        }
                    >
                        Delete
                    </button>
                ),
            },
        ],
        [onEdit, onDelete]
    );

    const defaultColDef = useMemo(
        () => ({
            resizable: true,
            sortable: true,
        }),
        []
    );

    return (
        <div
            style={{
                height: 400,
                width: '100%',
            }}
        >
            <AgGridReact
                rowData={todos}
                columnDefs={columnDefs}
                defaultColDef={defaultColDef}
                overlayNoRowsTemplate="<span>No ToDos found</span>"
                stopEditingWhenCellsLoseFocus={true}
                suppressCellFocus={true}
                getRowClass={(params) =>
                    isTodoOverdue(params.data)
                        ? 'todo-grid-overdue'
                        : ''
                }
            />
        </div>
    );
}