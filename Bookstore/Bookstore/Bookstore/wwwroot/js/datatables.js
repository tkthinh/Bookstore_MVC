let dataTable;

$(document).ready(function () {
  loadDataTable();
});

function loadDataTable() {
  dataTable = $('#tblData').DataTable({
    "ajax": '/admin/product/getall',
    "columns": [
      { data: "title", "width": "25%" },
      { data: "isbn", "width": "15%" },
      { data: "author", "width": "10%" },
      { data: "category.name", "width": "15%" },
      { data: "listPrice", "width": "10%" },
      {
        data: "id",
        "render": (data) => {
          return `<div class='w-75 btn-group' role='group'>
            <a href='/admin/product/upsert?id=${data}' class='btn btn-primary mx-2'><i class='bi bi-pencil-square'></i>Edit</a>
            <a onClick=handleDelete('/admin/product/delete/${data}') class='btn btn-danger mx-2'><i class='bi bi-trash-fill'></i>Delete</a>
          </div>
          `
        },
        "width": "20%"
      }
    ]
  },
  )
};

function handleDelete(url) {
  Swal.fire({
    title: "Are you sure?",
    text: "You won't be able to revert this!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#3085d6",
    cancelButtonColor: "#d33",
    confirmButtonText: "Yes, delete it!"
  }).then((result) => {
    if (result.isConfirmed) {
      $.ajax({
        url: url,
        type: 'DELETE',
        success: (data) => {
          dataTable.ajax.reload();
        }
      })
      Swal.fire({
        title: "Deleted!",
        text: "Your file has been deleted.",
        icon: "success"
      });
    }
  });
}