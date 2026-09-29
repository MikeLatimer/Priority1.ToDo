export const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000';

const TODOS_URL = `${API_BASE_URL}/todos`;
const TASKS_URL = `${API_BASE_URL}/tasks`;

async function handle(res) {
    if (!res.ok) {
        let message = `Request failed: ${res.status} ${res.statusText}`;

        try {
            const error = await res.json();

            if (error.errors) {
                const validationMessages = Object.values(error.errors)
                    .flat();

                if (validationMessages.length > 0) {
                    message = validationMessages.join(' ');
                }
            } else if (error.title) {
                message = error.title;
            } else if (error.message) {
                message = error.message;
            }
        } catch {
            // Keep the default message if the response is not JSON.
        }

        throw new Error(message);
    }

    return res.status === 204 ? null : res.json();
}


// Task Items

export function getTaskItems() {
  return fetch(TASKS_URL).then(handle);
}

export function createTaskItem({ name }) {
  return fetch(TASKS_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name }),
  }).then(handle);
}

export function updateTaskItem(id, { name }) {
  return fetch(`${TASKS_URL}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name }),
  }).then(handle);
}

export function deleteTaskItem(id) {
  return fetch(`${TASKS_URL}/${id}`, {
    method: 'DELETE',
  }).then(handle);
}


// Todos

export function getTodos(taskItemId) {
    return fetch(`${TODOS_URL}?taskItemId=${taskItemId}`).then(handle);
}

export function createTodo({
    title,
    isComplete = false,
    taskItemId,
    dueDate = null,
}) {
    return fetch(TODOS_URL, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            title,
            isComplete,
            taskItemId,
            dueDate,
        }),
    }).then(handle);
}

export function updateTodo(
    id,
    {
        title,
        isComplete,
        taskItemId,
        dueDate,
    }
) {
    return fetch(`${TODOS_URL}/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            title,
            isComplete,
            taskItemId,
            dueDate,
        }),
    }).then(handle);
}

export function deleteTodo(id) {
    return fetch(`${TODOS_URL}/${id}`, { method: 'DELETE' }).then(handle);
}