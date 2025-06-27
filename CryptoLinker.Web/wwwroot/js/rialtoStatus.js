async function rialtoStatus() {
            const status = document.getElementById("rialto-status");
            const time = document.getElementById("rialto-time");
            let latencyText = "-";
            try {
                const start = performance.now(); // засекаем время
                const res = await fetch(window.rialto.status, { cache: "no-store" }); // чекаем бинанс на дотсупность
                if (!res.ok) throw new Error("Bad response");
                const data = await res.json();
                const latency = Math.round(performance.now() - start); // считаем сколько прошло до получения ответа
                if (data.isOnline) {
                    status.innerHTML = '<span style="color:green">доступен</span>';
                    latencyText = `${latency} мс`;
                } else {
                    status.innerHTML = '<span style="color:deeppink">недоступен</span>';
                }
            } catch (err) {
                status.innerHTML = '<span style="color:red">ошибка</span>';
            }
            time.innerText = latencyText;
        }

rialtoStatus();

setInterval(rialtoStatus, 1000);