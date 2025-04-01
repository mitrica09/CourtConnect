let timerInterval;

function formatTime(secs) {
    const minutes = Math.floor(secs / 60).toString().padStart(2, '0');
    const seconds = (secs % 60).toString().padStart(2, '0');
    return `${minutes}:${seconds}`;
}

function getElapsedSeconds() {
    const startTime = localStorage.getItem("matchTimerStartTime");
    if (!startTime) return 0;
    const start = new Date(parseInt(startTime));
    const now = new Date();
    const diff = Math.floor((now - start) / 1000);
    return diff;
}

function updateTimerDisplay() {
    const elapsed = getElapsedSeconds();
    $('#matchTimer').text(formatTime(elapsed));
}

function startTimer() {
    // 🛑 Resetăm startTime la fiecare start
    localStorage.setItem("matchTimerStartTime", Date.now());
    localStorage.setItem("matchTimerRunning", "true");

    updateTimerDisplay();
    timerInterval = setInterval(updateTimerDisplay, 1000);
}

function stopTimer() {
    clearInterval(timerInterval);
    localStorage.setItem("matchTimerRunning", "false");
    localStorage.removeItem("matchTimerStartTime"); // ✅ Reset complet
}

$(document).ready(function () {
    const isRunning = localStorage.getItem("matchTimerRunning") === "true";

    if (isRunning) {
        $('#startMatchBtn').hide();
        $('#stopMatchBtn').show();
        updateTimerDisplay(); // să actualizeze înainte de interval
        timerInterval = setInterval(updateTimerDisplay, 1000);
    } else {
        $('#startMatchBtn').show();
        $('#stopMatchBtn').hide();
        $('#matchTimer').text("00:00");
    }

    $('#startMatchBtn').on('click', function () {
        if (localStorage.getItem("matchTimerStartTime")) {
            if (!confirm("Cronometrul va fi resetat. Ești sigur că vrei să pornești din nou?")) {
                return;
            }
        }

        $(this).hide();
        $('#stopMatchBtn').show();
        startTimer();
    });

    $('#stopMatchBtn').on('click', function () {
        $(this).hide();
        $('#startMatchBtn').show();
        stopTimer();
    });
});
