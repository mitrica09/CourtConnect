
    const allSideMenu = document.addEventListener("DOMContentLoaded", function () {
            // 🔹 Activează meniul selectat
            const allSideMenu = document.querySelectorAll('#sidebar .side-menu.top li a');

            allSideMenu.forEach(item => {
                const li = item.parentElement;

    item.addEventListener('click', function () {
        allSideMenu.forEach(i => i.parentElement.classList.remove('active'));
    li.classList.add('active');
                });
            });

    // 🔹 Expandează și restrânge meniul lateral
    const menuBar = document.querySelector('#content nav .bx.bx-menu');
    const sidebar = document.getElementById('sidebar');

    menuBar.addEventListener('click', function () {
        sidebar.classList.toggle('hide');
            });

    // 🔹 Dark Mode Toggle
    const switchMode = document.getElementById('switch-mode');

    switchMode.addEventListener('change', function () {
        document.body.classList.toggle('dark', this.checked);
            });
        });

