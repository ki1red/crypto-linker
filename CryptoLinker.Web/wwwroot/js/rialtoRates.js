let lastUpdateTime = null;
let lastTargets = [];

async function refreshRates() {
    const tbody = document.querySelector("#rates-body");
    try {
        const response = await fetch(window.rialto.rates);

        if (!response.ok || !response.headers.get("content-type")?.includes("application/json"))
            throw new Error("Bad response");

        const data = await response.json();

        if (!data || Object.keys(data).length === 0)
            throw new Error("Empty result");

        // Сбор всех заголовков валют
        const allTargets = new Set();
        for (const from in data) {
            for (const to in data[from]) {
                allTargets.add(to);
            }
        }
        const targets = Array.from(allTargets);
        if (targets.length > 0) {
            lastTargets = targets;
        }

        // Перегенерация таблцы
        tbody.innerHTML = "";
        for (const from in data) {
            const row = document.createElement("tr");
            row.innerHTML = `<td>${from}</td>`;
            for (const to of lastTargets) {
                const value = data[from][to];
                row.innerHTML += `<td>${value !== undefined ? value.toFixed(6) : "-"}</td>`;
            }
            tbody.appendChild(row);
        }

        // Обновляем время
        lastUpdateTime = new Date();
        document.getElementById("last-updated").style.display = "block";
        document.getElementById("seconds-ago").innerText = "0";
    } catch (err) {
        console.warn("Курсы не получены: ", err);

        if (!lastUpdateTime) {
            tbody.innerHTML = `<td colspan="1000" style="text-align:center; font-style:italic; color:#666;">Загрузка</td>`;
        }
    }
}

function updateTimer() {
    if (!lastUpdateTime) return;
    const seconds = Math.floor((new Date() - lastUpdateTime) / 1000);
    document.getElementById("seconds-ago").innerText = seconds;
}

refreshRates();

setInterval(refreshRates, 1000);
setInterval(updateTimer, 1000);