import {
    toastIcons
} from "./toast-icons.js";

function buildToast(type, message) {
    const icon = toastIcons[type] ?? toastIcons.info;

    return `
        <div class="max-w-xs rounded-xl border border-gray-200 bg-white shadow-lg dark:border-neutral-700 dark:bg-neutral-800" role="alert" tabindex="-1" aria-labelledby="hs-toast-warning-example-label">
            <div class="flex p-4">
            <div class="shrink-0">
                ${icon}
            </div>
            <div class="ms-3">
                <p id="hs-toast-warning-example-label" class="text-sm text-gray-700 dark:text-neutral-400">${message}</p>
            </div>
            </div>
        </div>
    `;
}

export function showToast(type, message) {
    Toastify({
        text: buildToast(type, message),
        escapeMarkup: false,
        duration: 3000,
        close: true,
        gravity: "top",
        position: "right"
    }).showToast();
}