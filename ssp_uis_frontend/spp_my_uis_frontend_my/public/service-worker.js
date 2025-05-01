const CACHE_NAME = 'my-chamber-v2';
const urlsToCache = [
    '/',
    '/index.html',
];

// Install the Service Worker and Cache Resources
self.addEventListener('install', (event) => {
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then((cache) => {
                console.log('Opened cache');
                return cache.addAll(urlsToCache);
            })
            .catch((error) => {
                console.error('Cache open failed:', error);
            })
    );
});

// Activate Event: Remove old caches if necessary
self.addEventListener('activate', (event) => {
    const cacheWhitelist = [CACHE_NAME];

    event.waitUntil(
        caches.keys().then((cacheNames) => {
            return Promise.all(
                cacheNames.map((cacheName) => {
                    if (!cacheWhitelist.includes(cacheName)) {
                        console.log('Deleting old cache:', cacheName);
                        return caches.delete(cacheName);
                    }
                })
            );
        })
            .catch((error) => {
                console.error('Activation failed:', error);
            })
    );
});
