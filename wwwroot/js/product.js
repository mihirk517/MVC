$(document).ready(function () {
    loadDataTable()
});

function loadDataTable() {
    $('#tblData').DataTable({
        "ajax":{ URL: '/product/getall' },
    });
}

