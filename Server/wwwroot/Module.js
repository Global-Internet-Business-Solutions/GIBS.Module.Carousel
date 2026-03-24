/* Module Script */
var GIBS = GIBS || {};

GIBS.Carousel = {
    preloadImages: function (carousel) {
        var urls = new Set();
        carousel.querySelectorAll('.carousel-item img').forEach(function (img) {
            var src = img.getAttribute('src');
            if (src) {
                urls.add(src);
                img.setAttribute('loading', 'eager');
                img.setAttribute('decoding', 'async');
            }
        });

        urls.forEach(function (url) {
            var preloaded = new Image();
            preloaded.src = url;
        });
    },

    initMultiItem: function (selector, minPerSlide) {
        var carousel = (typeof selector === 'string')
            ? document.querySelector(selector)
            : selector;
        if (!carousel) return;

        minPerSlide = minPerSlide
            || parseInt(carousel.getAttribute('data-min-per-slide'), 10)
            || 5;

        // Remove any previously cloned children so re-calls are safe
        carousel.querySelectorAll('[data-cloned]').forEach(function (el) {
            el.remove();
        });

        // Clone siblings into each slide
        var items = carousel.querySelectorAll('.carousel-item');
        items.forEach(function (el) {
            var next = el.nextElementSibling;
            for (var i = 1; i < minPerSlide; i++) {
                if (!next) next = items[0];
                var clone = next.cloneNode(true);
                var child = clone.children[0];
                if (!child) {
                    next = next.nextElementSibling;
                    continue;
                }
                child.setAttribute('data-cloned', 'true');
                child.querySelectorAll('img').forEach(function (img) {
                    img.setAttribute('loading', 'eager');
                    img.setAttribute('decoding', 'async');
                });
                el.appendChild(child);
                next = next.nextElementSibling;
            }
        });

        GIBS.Carousel.preloadImages(carousel);

        // Inject scoped responsive CSS for column width and slide transitions
        var pct = parseFloat((100 / minPerSlide).toFixed(4));
        var styleId = 'gibs-carousel-style-' + (carousel.id || 'default');
        var existing = document.getElementById(styleId);
        if (existing) existing.remove();

        var scope = carousel.id ? '#' + carousel.id : '.carousel';
        var style = document.createElement('style');
        style.id = styleId;
        style.textContent =
            '/* Mobile - 1 item */\n' +
            scope + ' .carousel-item .col-6 { flex: 0 0 auto; width: 100%; max-width: 100%; }\n' +
            '\n' +
            '/* Tablet - 2 items */\n' +
            '@media (min-width: 576px) {\n' +
            '  ' + scope + ' .carousel-item .col-6 { width: 50%; max-width: 50%; }\n' +
            '}\n' +
            '\n' +
            '/* Desktop - configured items */\n' +
            '@media (min-width: 992px) {\n' +
            '  ' + scope + ' .carousel-item .col-6 { flex: 0 0 auto; width: ' + pct + '%; max-width: ' + pct + '%; }\n' +
            '  ' + scope + ' .carousel-inner .carousel-item-end.active,\n' +
            '  ' + scope + ' .carousel-inner .carousel-item-next { transform: translateX(' + pct + '%) !important; }\n' +
            '  ' + scope + ' .carousel-inner .carousel-item-start.active,\n' +
            '  ' + scope + ' .carousel-inner .carousel-item-prev { transform: translateX(-' + pct + '%) !important; }\n' +
            '  ' + scope + ' .carousel-inner .carousel-item-end,\n' +
            '  ' + scope + ' .carousel-inner .carousel-item-start { transform: translateX(0) !important; }\n' +
            '}';
        document.head.appendChild(style);
    },

    initAll: function () {
        document.querySelectorAll('.carousel[data-min-per-slide]').forEach(function (carousel) {
            GIBS.Carousel.initMultiItem(carousel);
        });
    },

    observeDOM: function () {
        var observer = new MutationObserver(function (mutations) {
            mutations.forEach(function (mutation) {
                if (mutation.type === 'childList' || mutation.type === 'attributes') {
                    var carousels = document.querySelectorAll('.carousel[data-min-per-slide]');
                    carousels.forEach(function (carousel) {
                        if (!carousel.hasAttribute('data-gibs-initialized')) {
                            GIBS.Carousel.initMultiItem(carousel);
                            carousel.setAttribute('data-gibs-initialized', 'true');
                        }
                    });
                }
            });
        });

        var config = {
            childList: true,
            subtree: true,
            attributes: true,
            attributeFilter: ['data-min-per-slide']
        };
        observer.observe(document.body, config);
    }
};

(function () {
    function init() {
        GIBS.Carousel.initAll();
        GIBS.Carousel.observeDOM();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init, { once: true });
    } else {
        init();
    }
})();