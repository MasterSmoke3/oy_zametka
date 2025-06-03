self.addEventListener('notificationclick', function(event) {
    const data = event.notification.data || {};
    event.notification.close();

    if (event.action === 'snooze') {
        setTimeout(() => {
            self.registration.showNotification(data.title, {
                body: data.body,
                icon: '/favicon.ico',
                data: data
            });
        }, 5 * 60 * 1000); // через 5 минут
    } else {
        event.waitUntil(clients.openWindow('/'));
    }
});
