document.getElementById('searchInput').addEventListener('keyup', function () {
    let searchValue = this.value.toLowerCase();
    let rows = document.querySelectorAll('#tableBody tr');

    rows.forEach(row => {
        let content = row.textContent.toLowerCase();
        row.style.display = content.includes(searchValue) ? '' : 'none';
    });
});