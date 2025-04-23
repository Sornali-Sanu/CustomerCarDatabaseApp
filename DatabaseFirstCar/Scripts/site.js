$('#btnPlus').click(function (e) {
  e.preventDefault();
  var quContainer = $("#QualificationContainer");
  $.ajax({
    url: "/Customers/AddNewModel",
    type: "GET",
    success: function (data) {
      quContainer.append(data);
    }
  })
})
function readURL(input) {
  if (input.files && input.files[0]) {
    var reader = new FileReader();
    reader.onload = function (e) {
      $('#ImgFile').attr('src', e.target.result);
    };
    reader.readAsDataURL(input.files[0])
  }
}
$(document).on("click", "#btnDeleteQualification", function (e) {
  e.preventDefault();
  $(this).parent().parent().remove();
})