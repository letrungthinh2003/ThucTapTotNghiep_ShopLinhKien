document.addEventListener('DOMContentLoaded', () => {
    // Xử lý sự kiện collapse cho danh mục
    const categoryToggle = document.querySelector('[data-bs-target="#categoryMenu"]');
    if (categoryToggle) {
        categoryToggle.addEventListener('click', (e) => {
            e.preventDefault();
            const collapseElement = document.getElementById('categoryMenu');
            if (collapseElement) {
                const bsCollapse = new bootstrap.Collapse(collapseElement, {
                    toggle: true
                });
            }
        });
    }
});