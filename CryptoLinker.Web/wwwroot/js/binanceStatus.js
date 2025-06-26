async function binanceStatus() {
            const status = document.getElementById("binance-status");
            const time = document.getElementById("binance-time");
            let latencyText = "-"; // для скорости отклика
            try {
                const start = performance.now(); // засекаем время
                const res = await fetch(window.api.binanceStatus, { cache: "no-store" }); // чекаем бинанс на дотсупность
                if (!res.ok) throw new Error("Bad response");
                const data = await res.json();
                const latency = Math.round(performance.now() - start); // считаем сколько прошло до получения ответа
                if (data.isOnline) {
                    status.innerHTML = '<span>есть доступ</span>'; // добавить цвета
                    latencyText = `${latency} мс`;
                } else {
                    status.innerHTML = '<span>нет доступа</span>';
                }
            } catch (err) {
                status.innerHTML = '<span>ошибка</span>';
            }
            time.innerText = latencyText;
        }

setInterval(binanceStatus, 1000);