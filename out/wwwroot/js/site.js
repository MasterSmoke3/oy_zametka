// === Настройки по умолчанию ===
if (localStorage.getItem("notificationsEnabled") === null) {
    localStorage.setItem("notificationsEnabled", "true");
}
if (localStorage.getItem("soundEnabled") === null) {
    localStorage.setItem("soundEnabled", "true");
}
if (localStorage.getItem("snoozeEnabled") === null) {
    localStorage.setItem("snoozeEnabled", "true");
}

// === Воспроизведение звука ===
function playReminderSound() {
    if (localStorage.getItem("soundEnabled") === "true") {
        const audio = new Audio("/sound/reminder.mp3");
        audio.play().catch(() => {});
    }
}

// === Регистрация Service Worker ===
let serviceWorkerReady = false;
navigator.serviceWorker?.register('/service-worker.js')
    .then(reg => {
        console.log("Service Worker зарегистрирован");
        serviceWorkerReady = reg;
    })
    .catch(err => console.error("Ошибка регистрации Service Worker", err));

// === Показ уведомления ===
function showNotification(title, body, data = {}) {
    if (Notification.permission !== "granted") return;

    const options = {
        body: body,
        icon: "/favicon.ico",
        data: data
    };

    if (localStorage.getItem("snoozeEnabled") === "true") {
        options.actions = [{ action: "snooze", title: "Отложить на 5 мин" }];
    }

    if (serviceWorkerReady && serviceWorkerReady.showNotification) {
        serviceWorkerReady.showNotification(title, options);
    } else if (navigator.serviceWorker?.ready) {
        navigator.serviceWorker.ready.then(reg => {
            reg.showNotification(title, options);
        });
    } else {
        new Notification(title, options);
    }

    playReminderSound();
}

// === Проверка разрешения на уведомления ===
if (Notification && Notification.permission !== "granted") {
    Notification.requestPermission();
}
