document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('select-all').addEventListener('change', function () {
        var checkboxes = document.querySelectorAll('input[name="selectedUsers"]');
        for (var checkbox of checkboxes) {
            checkbox.checked = this.checked;
        }
    });
});
