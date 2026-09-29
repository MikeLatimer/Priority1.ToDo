import { useMemo } from 'react';
import { AgGridReact } from 'ag-grid-react';

export default function TodoList({
    todos,
    onEdit,
    onDelete,
}) {
    const columnDefs = useMemo(
        () => [
            {
                headerName: 'Edit',
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
                // TODO: Make due date editable, will need to update to DateOnl
                field: 'dueDate',
                headerName: 'Due Date',
                width: 170,
                sortable: true,
                valueFormatter: (params) =>
                    params.value
                        ? new Date(params.value).toLocaleDateString()
                        : '',
            },
            {
                field: 'createDate',
                headerName: 'Create Date',
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
                // TODO: Allow users to update Todo status between Open and Closed.
                // Overdue status should be calculated automatically based on the due date.
                field: 'isComplete',
                headerName: 'Status',
                width: 150,
                editable: true,
                cellEditor: 'agSelectCellEditor',
                cellEditorParams: {
                    values: [true, false],
                },
                valueFormatter: (params) =>
                    params.value
                        ? 'Complete'
                        : 'Open',
                cellRenderer: (params) => {
                    if (params.value) {
                        return (
                            <span className="badge text-bg-success">
                                Complete
                            </span>
                        );
                    }

                    const isOverdue =
                        params.data.dueDate &&
                        new Date(
                            params.data.dueDate
                        ) < new Date();

                    return (
                        <span
                            className={
                                isOverdue
                                    ? 'badge text-bg-danger'
                                    : 'badge text-bg-secondary'
                            }
                        >
                            {isOverdue
                                ? 'Overdue'
                                : 'Open'}
                        </span>
                    );
                },
                onCellValueChanged: (params) => {
                    if (
                        params.oldValue !==
                        params.newValue
                    ) {
                        onEdit({
                            ...params.data,
                            isComplete:
                                params.newValue === true ||
                                params.newValue === 'true',
                        });
                    }
                },
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
                getRowClass={(params) => {
                    const todo = params.data;

                    if (
                        !todo?.isComplete &&
                        todo?.dueDate &&
                        new Date(todo.dueDate) <
                        new Date()
                    ) {
                        return 'todo-grid-overdue';
                    }

                    return '';
                }}
            />
        </div>
    );
}