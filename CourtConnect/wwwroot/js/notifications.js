document.addEventListener("DOMContentLoaded", function () {
    var notificationMessage = document.getElementById("notificationMessage")?.value;
    var notificationType = document.getElementById("notificationType")?.value;

    if (notificationMessage) {
        var alertElement = document.getElementById("notificationAlert");
        var alertTitle = document.getElementById("notificationAlertTitle");
        var alertMessage = document.getElementById("notificationAlertMessage");

        // Configurează titlul si stilul în functie de tipul notificarii
        if (notificationType === "success") {
            alertElement.style.backgroundColor = "#4CAF50"; // Verde
            alertTitle.textContent = "Succes!";
        } else if (notificationType === "error") {
            alertElement.style.backgroundColor = "#F44336"; // Rosu
            alertTitle.textContent = "Eroare!";
        } else {
            alertElement.style.backgroundColor = "#2196F3"; // Albastru
            alertTitle.textContent = "Notificare";
        }

        // Setează mesajul notificării
        alertMessage.textContent = notificationMessage;

        // Afi?ează alerta
        alertElement.classList.remove("hidden");
        alertElement.classList.add("visible");

        // Ascunde alerta automat după 3 secunde
        setTimeout(function () {
            alertElement.classList.remove("visible");
            alertElement.classList.add("hidden");
        }, 3000);
    }
});