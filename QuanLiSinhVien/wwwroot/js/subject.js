$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url":'/admin/subject/getall'
        },
        "columns": [
            { "data": "name" },
            { "data": "credits" },  
            { "data": "teach" },  
            { "data": "student" },  
            {
                "data": "id",
                "render": function (data) {
                    return `
                             <div class="text-center">
                                <a href="/Admin/subject/upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onClick=Delete("/Admin/subject/delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </div>                           
                            
                           `
                }
            }


        ]

    });
}); 
const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-success',
        cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
})
function Delete(url) {
    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        swalWithBootstrapButtons.fire(
                            'Deleted!',
                            data.message,
                            'success'
                        );
                        $('#dataTable').DataTable().ajax.reload();
                    }
                    else {
                        swalWithBootstrapButtons.fire(
                            'Error',
                            data.message,
                            'error'
                        )
                    }
                }

            })

        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Your record is safe :)',
                'error'
            )
        }
    })
}



