$(document).ready(function () {


    LoadInfo();



})

function LoadInfo() {
    var raw = '';
    $.ajax({
        url: '/student/home/GetAllSubject',
        success: function (data) {
            console.log(data)
            for (var i = 0; i < data.length; i++) {
                raw += `
                    <div class="card mt-3">
                            <div class="card-body">
                                <h5 class="card-title"><strong>Subject Name: ${data[i].subjectName} </strong></h5>
                                <div class="row">
                                   `
                var ScoreRaw = '';

                for (var y = 0; y < data[i].score.length; y++) {
                    ScoreRaw +=
                        `
                            <div class="col-lg-2 col-md-4 col-sm-6">
                                   <label>${data[i].score[y].examtype}: </label><span> ${data[i].score[y].mark}</span>
                             </div>


                            `
                }
                raw += ScoreRaw + `
                                </div>
                                <h3> Average score: ${data[i].avgScore}</h3>
                                <a onclick=CancelSubject("${data[i].subjectid}") class="btn btn-danger"> Cancel</a>
                            </div>
                        </div>`
            }
            $('#studentScoreInfo').html(raw);
            console.log(raw)
        }

    })

}
function CancelSubject(id) {
    $.ajax({
        url: '/Student/home/CancelSubject/' + id,
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                LoadInfo();
            }
            else {
                toastr.error(data.message);
            }
        }
    })
}

$('#regisSubjectForm').on('submit', function (e) {
    var id = $('#subjectId').val();
    $.ajax({
        url: '/Student/Home/RegisterSubject/' + id,
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                LoadInfo();
            }
            else {
                toastr.error(data.message);
            }
        }
    })
    e.preventDefault();


})