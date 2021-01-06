$(document).ready(function () {


    LoadInfo();



})

function LoadInfo() {
    var raw = '';
    var subject = $('#subjectId').val();
    $.ajax({
        url: '/teacher/subject/getinfo/' + subject,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                raw +=`
                    <div class="card">
                            <div class="card-body">
                                <h5 class="card-title"><strong>Student Name: ${data[i].studentName} </strong></h5>
                                <div class="row">
                                   `
                var ScoreRaw = '';
                
                for (var y = 0; y < data[i].score.length; y++) {
                    ScoreRaw +=
                             `
                            <div class="col-lg-2 col-md-4 col-sm-6">
                                   <label>${data[i].score[y].examType.name}: </label><span> ${data[i].score[y].score}</span>
                             </div>


                            `
                }
                 raw+=ScoreRaw+ `
                                </div>
                                <h3>Average Score: ${data[i].avgScore}</h3>
                                <a href="/Teacher/Subject/MarkStudent?subjectId=${data[i].subjectId}&studentId=${data[i].studentId}" class="btn btn-primary">Update Score</a>
                            </div>
                        </div>`
            }
            $('#studentScoreInfo').html(raw);
        }
        
    })
    
}