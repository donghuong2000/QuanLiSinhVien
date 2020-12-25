$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url":'/teacher/subject/getall'
        },
        "columns": [
            { "data": "name" },
            { "data": "credits" },  
            { "data": "student" },  
            {
                "data": "id",
                "render": function (data) {
                    return `
                             <div class="text-center">
                                <a href="/teacher/subject/info?id=${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-info-circle"></i>
                                </a>
                                <a onClick=Delete("/teacher/subject/delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </div>                           
                            
                           `
                }
            }


        ]

    });
}); 

$('#regisSubjectForm').on('submit', function (e) {
    var data = $('#subjectId').val();
    $.ajax({
        url: '/teacher/subject/Registration',
        data: { id: data },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message)
                $('#dataTable').DataTable().ajax.reload();
            }
            else {
                toastr.error(response.message)
            }
        }
    })
    
    $('#registerSubject').modal('hide');
    e.preventDefault();

})



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



