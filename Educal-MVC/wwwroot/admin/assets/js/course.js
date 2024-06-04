$(function () {
    $(document).on("click", "#edit-area .img-delete", function () {
        let imageId = parseInt($(this).attr("data-id"));
        let courseId = parseInt($(this).attr("data-courseId"));

        let data = { courseId, imageId }

        $.ajax({
            type: "POST",
            url: `/admin/course/deletecourseimage`,
            data: data,
            success: function () {
                $(`[data-id=${imageId}]`).closest(".col-3").remove();
            }
        });
    })

    $(document).on("click", "#edit-area .set-main", function () {
        let imageId = parseInt($(this).attr("data-id"));
        let courseId = parseInt($(this).attr("data-courseId"));

        let data = { courseId, imageId }

        $.ajax({
            type: "POST",
            url: `/admin/course/setmainimage`,
            data: data,
            success: function () {
                $(".image-main").removeClass("image-main");
                $(`[data-id=${imageId}]`).closest(".col-3").find("img").addClass("image-main");
                $(".hide-btn")
                .addClass("show-btn")
                .removeClass("hide-btn");
                $(`[data-id=${imageId}]`).closest(".col-3")
                .addClass("hide-btn")
                .removeClass("show-btn");
            }
        });
    })
})