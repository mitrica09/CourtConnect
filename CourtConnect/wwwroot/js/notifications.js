document.addEventListener("DOMContentLoaded", function () {
    var notificationMessage = document.getElementById("notificationMessage")?.value;
    var notificationType = document.getElementById("notificationType")?.value;

    if (notificationMessage) {
        var alertElement = document.getElementById("notificationAlert");
        var alertTitle = document.getElementById("notificationAlertTitle");
        var alertMessage = document.getElementById("notificationAlertMessage");

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

        alertMessage.textContent = notificationMessage;

        alertElement.classList.remove("hidden");
        alertElement.classList.add("visible");

        setTimeout(function () {
            alertElement.classList.remove("visible");
            alertElement.classList.add("hidden");
        }, 3000);
    }
});