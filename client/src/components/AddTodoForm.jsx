import { useState } from 'react';

export default function AddTodoForm({ onAdd }) {
    const [title, setTitle] = useState('');
    const [dueDate, setDueDate] = useState('');

    const today = new Date().toISOString().split('T')[0];

    async function handleSubmit(e) {
        e.preventDefault();

        const trimmed = title.trim();

        if (!trimmed) {
            return;
        }

        await onAdd({
            title: trimmed,
            dueDate: dueDate || null,
        });

        setTitle('');
        setDueDate('');
    }

    return (
        <form onSubmit={handleSubmit}>
            <div className="row g-3 align-items-end">
                <div className="col-md-7">
                    <label
                        htmlFor="todo-title"
                        className="form-label"
                    >
                        Todo Title
                    </label>

                    <input
                        id="todo-title"
                        type="text"
                        className="form-control"
                        placeholder="What needs doing?"
                        value={title}
                        onChange={(e) =>
                            setTitle(e.target.value)
                        }
                    />
                </div>

                <div className="col-md-3">
                    <label
                        htmlFor="todo-due-date"
                        className="form-label"
                    >
                        Due Date
                    </label>

                    <input
                        id="todo-due-date"
                        type="date"
                        className="form-control"
                        min={today}
                        value={dueDate}
                        onChange={(e) =>
                            setDueDate(e.target.value)
                        }
                    />
                </div>

                <div className="col-md-2">
                    <button
                        type="submit"
                        className="btn btn-priority w-100"
                    >
                        + Add Todo
                    </button>
                </div>
            </div>
        </form>
    );
}