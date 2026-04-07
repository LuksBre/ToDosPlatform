const userMenu = document.getElementById('userMenu');
const dropdown = document.getElementById('dropdown');

userMenu.addEventListener('click', (e) => {
    e.stopPropagation();
    dropdown.classList.toggle('active');
});

document.addEventListener('click', () => {
    dropdown.classList.remove('active');
});