import { useEffect, useState } from 'react';
import { AllCommunityModule } from 'ag-grid-community';
import { AgGridProvider } from 'ag-grid-react';

import Priority1Logo from './assets/Priority1Logo.webp';

import {
    getTaskItems,
    createTaskItem,
    updateTaskItem,
    deleteTaskItem,
    getTodos,
    createTodo,
    updateTodo,
    deleteTodo,
} from './api';

import TaskItemGrid from './components/TaskItemGrid';
import AddTodoForm from './components/AddTodoForm';
import TodoList from './components/TodoList';

const modules = [AllCommunityModule];

export default function App() {
    const [taskItems, setTaskItems] = useState([]);
    const [selectedTaskItem, setSelectedTaskItem] = useState(null);
    const [todos, setTodos] = useState([]);

    const [loading, setLoading] = useState(true);
    const [todosLoading, setTodosLoading] = useState(false);
    const [error, setError] = useState(null);

    useEffect(() => {
        loadTaskItems();
    }, []);

    async function loadTaskItems() {
        try {
            setLoading(true);
            setError(null);

            const data = await getTaskItems();
            setTaskItems(data);
        } catch (e) {
            setError(e.message);
        } finally {
            setLoading(false);
        }
    }

    async function handleAddTaskItem() {
        const name = window.prompt(
            'Enter a name for the new ToDo list:'
        );

        if (!name?.trim()) {
            return;
        }

        try {
            setError(null);

            const created = await createTaskItem({
                name: name.trim(),
            });

            setTaskItems((prev) => [
                ...prev,
                created,
            ]);
        } catch (e) {
            setError(e.message);
        }
    }

    async function handleEditTaskItem(taskItem) {
        try {
            setError(null);

            const updated = await updateTaskItem(
                taskItem.id,
                {
                    name: taskItem.name,
                }
            );

            setTaskItems((prev) =>
                prev.map((item) =>
                    item.id === updated.id
                        ? updated
                        : item
                )
            );
        } catch (e) {
            setError(e.message);
            await loadTaskItems();
        }
    }

    async function handleDeleteTaskItem(taskItem) {
        const confirmed = window.confirm(
            `Are you sure you want to delete "${taskItem.name}"?`
        );

        if (!confirmed) {
            return;
        }

        try {
            setError(null);

            await deleteTaskItem(taskItem.id);

            setTaskItems((prev) =>
                prev.filter(
                    (item) => item.id !== taskItem.id
                )
            );
        } catch (e) {
            setError(e.message);
        }
    }

    async function handleViewTodos(taskItem) {
        try {
            setTodosLoading(true);
            setError(null);

            setSelectedTaskItem(taskItem);

            const data = await getTodos(taskItem.id);

            setTodos(data);
        } catch (e) {
            setError(e.message);
        } finally {
            setTodosLoading(false);
        }
    }

    async function handleAddTodo({
        title,
        dueDate,
    }) {
        if (!selectedTaskItem) {
            return false;
        }

        try {
            setError(null);

            const created = await createTodo({
                title,
                isComplete: false,
                taskItemId: selectedTaskItem.id,
                dueDate,
            });

            setTodos((prev) => [
                ...prev,
                created,
            ]);

            return true;
        } catch (e) {
            setError(e.message);
            return false;
        }
    }

    async function handleEditTodo(todo) {
        try {
            setError(null);

            const updated = await updateTodo(
                todo.id,
                {
                    title: todo.title,
                    isComplete: todo.isComplete,
                    taskItemId: todo.taskItemId,
                    dueDate: todo.dueDate,
                }
            );

            setTodos((prev) =>
                prev.map((item) =>
                    item.id === updated.id
                        ? updated
                        : item
                )
            );
        } catch (e) {
            setError(e.message);

            if (selectedTaskItem) {
                const data = await getTodos(
                    selectedTaskItem.id
                );

                setTodos(data);
            }
        }
    }

    async function handleDeleteTodo(todo) {
        const confirmed = window.confirm(
            `Are you sure you want to delete "${todo.title}"?`
        );

        if (!confirmed) {
            return;
        }

        try {
            setError(null);

            await deleteTodo(todo.id);

            setTodos((prev) =>
                prev.filter(
                    (item) => item.id !== todo.id
                )
            );
        } catch (e) {
            setError(e.message);
        }
    }

    function handleBack() {
        setSelectedTaskItem(null);
        setTodos([]);
        setError(null);
    }

    if (selectedTaskItem) {
        return (
            <AgGridProvider modules={modules}>
                <>
                    <header className="app-header">
                        <div className="app-header-inner">
                            <img
                                src={Priority1Logo}
                                alt="Priority1"
                                className="app-logo"
                            />
                        </div>
                    </header>

                    <main className="app-content">
                        <div className="app">
                            <div className="mb-4">
                                <button
                                    type="button"
                                    className="btn btn-priority-dark"
                                    onClick={handleBack}
                                >
                                    ← Back to My Priorities
                                </button>
                            </div>

                            <div className="section-header">
                                <div>
                                    <div className="priority-label">
                                        Priority List:
                                    </div>

                                    <h2>
                                        {selectedTaskItem.name}
                                    </h2>
                                </div>
                            </div>

                            {error && (
                                <div className="alert alert-danger py-2">
                                    {error}
                                </div>
                            )}

                            <div className="todo-panel mb-4">
                                <AddTodoForm
                                    onAdd={handleAddTodo}
                                />
                            </div>

                            {todosLoading ? (
                                <p className="text-muted">
                                    Loading ToDos...
                                </p>
                            ) : (
                                <div className="grid-panel">
                                    <TodoList
                                        todos={todos}
                                        onEdit={handleEditTodo}
                                        onDelete={handleDeleteTodo}
                                    />
                                </div>
                            )}
                        </div>
                    </main>
                </>
            </AgGridProvider>
        );
    }

    return (
        <AgGridProvider modules={modules}>
            <>
                <header className="app-header">
                    <div className="app-header-inner">
                        <img
                            src={Priority1Logo}
                            alt="Priority1"
                            className="app-logo"
                        />
                    </div>
                </header>

                <main className="app-content">
                    <div className="app">
                        <section>
                            <div className="section-header">
                                <h2>My Priorities</h2>

                                <button
                                    type="button"
                                    className="btn btn-priority ms-auto"
                                    onClick={handleAddTaskItem}
                                >
                                    + New Priority
                                </button>
                            </div>

                            {error && (
                                <div className="alert alert-danger py-2">
                                    {error}
                                </div>
                            )}

                            {loading ? (
                                <p className="text-muted">
                                    Loading...
                                </p>
                            ) : (
                                <div className="grid-panel">
                                    <TaskItemGrid
                                        taskItems={taskItems}
                                        onEdit={handleEditTaskItem}
                                        onDelete={handleDeleteTaskItem}
                                        onAddTodo={handleViewTodos}
                                    />
                                </div>
                            )}
                        </section>
                    </div>
                </main>
            </>
        </AgGridProvider>
    );
}