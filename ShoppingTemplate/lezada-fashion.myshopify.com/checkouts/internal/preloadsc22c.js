
    (function() {
      var baseURL = "https://cdn.shopify.com/shopifycloud/checkout-web/assets/";
      var scripts = ["https://cdn.shopify.com/shopifycloud/checkout-web/assets/runtime.baseline.fr.3c78604390bba384620a.js","https://cdn.shopify.com/shopifycloud/checkout-web/assets/922.baseline.fr.f7cf671d5695adc84050.js","https://cdn.shopify.com/shopifycloud/checkout-web/assets/831.baseline.fr.70ad5a6e640d98983cab.js","https://cdn.shopify.com/shopifycloud/checkout-web/assets/681.baseline.fr.94e90f9c8d09c861763f.js","https://cdn.shopify.com/shopifycloud/checkout-web/assets/app.baseline.fr.4b9d0cc74f7b67291718.js","https://cdn.shopify.com/shopifycloud/checkout-web/assets/751.baseline.fr.b034168d5d5932189976.js","https://cdn.shopify.com/shopifycloud/checkout-web/assets/21.baseline.fr.04149fd5b11fbbdef71a.js","https://cdn.shopify.com/shopifycloud/checkout-web/assets/214.baseline.fr.953f18ae0bb61247681f.js","https://cdn.shopify.com/shopifycloud/checkout-web/assets/100.baseline.fr.a4f86ac8f0bbf8d9ab36.js","https://cdn.shopify.com/shopifycloud/checkout-web/assets/OnePage.baseline.fr.571b4063c216e1fbf1e6.js"];
      var styles = ["https://cdn.shopify.com/shopifycloud/checkout-web/assets/922.baseline.fr.79a72389146be45c9ca4.css","https://cdn.shopify.com/shopifycloud/checkout-web/assets/app.baseline.fr.f79e630f70b79519e81e.css","https://cdn.shopify.com/shopifycloud/checkout-web/assets/21.baseline.fr.9abd9872d9c712c0373e.css","https://cdn.shopify.com/shopifycloud/checkout-web/assets/268.baseline.fr.7831178b9ae7b8153f93.css"];
      var fontPreconnectUrls = [];
      var fontPrefetchUrls = [];
      var imgPrefetchUrls = [];

      function preconnect(url, callback) {
        var link = document.createElement('link');
        link.rel = 'dns-prefetch preconnect';
        link.href = url;
        link.crossOrigin = '';
        link.onload = link.onerror = callback;
        document.head.appendChild(link);
      }

      function preconnectAssets() {
        var resources = [baseURL].concat(fontPreconnectUrls);
        var index = 0;
        (function next() {
          var res = resources[index++];
          if (res) preconnect(res[0], next);
        })();
      }

      function prefetch(url, as, callback) {
        var link = document.createElement('link');
        if (link.relList.supports('prefetch')) {
          link.rel = 'prefetch';
          link.fetchPriority = 'low';
          link.as = as;
          if (as === 'font') link.type = 'font/woff2';
          link.href = url;
          link.crossOrigin = '';
          link.onload = link.onerror = callback;
          document.head.appendChild(link);
        } else {
          var xhr = new XMLHttpRequest();
          xhr.open('GET', url, true);
          xhr.onloadend = callback;
          xhr.send();
        }
      }

      function prefetchAssets() {
        var resources = [].concat(
          scripts.map(function(url) { return [url, 'script']; }),
          styles.map(function(url) { return [url, 'style']; }),
          fontPrefetchUrls.map(function(url) { return [url, 'font']; }),
          imgPrefetchUrls.map(function(url) { return [url, 'image']; })
        );
        var index = 0;
        (function next() {
          var res = resources[index++];
          if (res) prefetch(res[0], res[1], next);
        })();
      }

      function onLoaded() {
        preconnectAssets();
        prefetchAssets();
      }

      if (document.readyState === 'complete') {
        onLoaded();
      } else {
        addEventListener('load', onLoaded);
      }
    })();
  