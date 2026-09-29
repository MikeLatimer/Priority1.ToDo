import { useMemo } from 'react';
import { AgGridReact } from 'ag-grid-react';

export default function TaskItemGrid({
    taskItems,
    onEdit,
    onDelete,
    onAddTodo,
}) {
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
                                colKey: 'name',
                            });
                        }}
                    >
                        Edit
                    </button>
                ),
            },
            {
                field: 'name',
                headerName: 'Name',
                flex: 2,
                minWidth: 300,
                editable: true,
                cellEditor: 'agTextCellEditor',
                onCellValueChanged: (params) => {
                    if (params.oldValue !== params.newValue) {
                        onEdit({
                            ...params.data,
                            name: params.newValue,
                        });
                    }
                },
            },
            {
                field: 'isActive',
                headerName: 'Status',
                width: 140,
                cellRenderer: (params) => (
                    <span
                        className={
                            params.value
                                ? 'badge text-bg-success'
                                : 'badge text-bg-secondary'
                        }
                    >
                        {params.value ? 'Active' : 'Inactive'}
                    </span>
                ),
            },
            {
                field: 'createDate',
                headerName: 'Create Date',
                width: 170,
                sortable: true,
                valueFormatter: (params) =>
                    params.value
                        ? new Date(params.value).toLocaleDateString()
                        : '',
            },
            {
                headerName: 'Actions',
                width: 320,
                sortable: false,
                filter: false,
                cellRenderer: (params) => (
                    <div className="d-flex align-items-center gap-2 h-100">
                        <button
                            type="button"
                            className="btn btn-priority btn-sm"
                            onClick={() => onAddTodo(params.data)}
                        >
                            Manage Todos
                        </button>

                        <button
                            type="button"
                            className="btn btn-danger btn-sm"
                            onClick={() => onDelete(params.data)}
                        >
                            Delete
                        </button>
                    </div>
                ),
            },
        ],
        [onEdit, onDelete, onAddTodo]
    );

    const defaultColDef = useMemo(
        () => ({
            resizable: true,
            sortable: true,
        }),
        []
    );

    return (
        <div style={{ height: 400, width: '100%' }}>
            <AgGridReact
                rowData={taskItems}
                columnDefs={columnDefs}
                defaultColDef={defaultColDef}
                overlayNoRowsTemplate="<span>No records found</span>"
                stopEditingWhenCellsLoseFocus={true}
                suppressCellFocus={true}
            />
        </div>
    );
}