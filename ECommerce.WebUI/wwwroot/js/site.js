$(document).ready(function () {
    $("#searchInput").on("keyup", function () {
        var query = $(this).val();
        var category = $("#currentCategory").val();

        $.ajax({
            url: '/Product/SearchProducts',
            type: 'GET',
            data: { query: query, category: category },
            success: function (data) {
                $("#productList").html(data);
            },
            error: function () {
                alert("An error occurred while searching products.");
            }
        });
    });
});
