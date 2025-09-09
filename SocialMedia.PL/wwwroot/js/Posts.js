
<script>
    function loadComments(postId) {
        $.ajax({
            url: `/Comment/GetAllComments?id=${postId}`,
            type: "GET",
            success: function (data) {
                let container = $(`#comments-${postId}`);
                container.empty();
                if (data && data.length > 0) {
                    data.forEach(c => {
                        container.append(`
                            <div class="d-flex mb-3" id="comment-${c.id}">
        <div class="flex-grow-1">
            <div class="p-2 bg-white rounded shadow-sm border" id="comment-text-${c.id}">
                <strong class="d-block">${currentUser}</strong>
                <span id="comment-content-${c.id}">${c.content}</span>
            </div>

            <!-- فورم التعديل -->
            <form class="edit-comment-form d-none mt-2" data-id="${c.id}">
                <input type="hidden" name="ID" value="${c.id}" />
                <input type="text" name="Content" class="form-control mb-2" value="${c.content}" />
                <button type="submit" class="btn btn-sm btn-primary">Save</button>
                <button type="button" class="btn btn-sm btn-secondary cancel-edit" data-id="${c.id}">Cancel</button>
            </form>

            <div class="d-flex align-items-center mt-1 small text-muted">
                <button class="btn btn-link btn-sm p-0 me-3 text-decoration-none">Like</button>
                <button class="btn btn-link btn-sm p-0 me-3 text-decoration-none">Reply</button>
                <button class="btn btn-link btn-sm p-0 me-3 text-decoration-none edit-btn" data-id="${c.id}">Edit</button>
                <span>· Just now</span>
            </div>
                <button type="button" class="btn btn-link text-danger delete-comment-btn" data-id="${c.id}">
        <i class="bi bi-trash me-2"></i> Delete
    </button>


            <div class="replies mt-2 ps-4 border-start" id="replies-${c.id}">
                <!-- الردود هتتحمل هنا -->
            </div>
        </div>
    </div>
                        `);
                    });
                } else {
                    container.append(`<p class="text-muted">No comments yet.</p>`);
                }
            },
            error: function () {
                console.log("Error loading comments.");
            }
        });
    }
    // إظهار الفورم عند الضغط على Edit
    $(document).on("click", ".edit-btn", function () {
        let id = $(this).data("id");
        $(`#comment-text-${id}`).hide();
        $(`form[data-id='${id}']`).removeClass("d-none");
    });

    // إلغاء التعديل
    $(document).on("click", ".cancel-edit", function () {
        let id = $(this).data("id");
        $(`#comment-text-${id}`).show();
        $(`form[data-id='${id}']`).addClass("d-none");
    });

    // إرسال التعديل
    $(document).on("submit", ".edit-comment-form", function (e) {
        e.preventDefault();
    let form = $(this);
    let id = form.data("id");
    let content = form.find("input[name='Content']").val();

    $.ajax({
        url: "/Comment/UpdateComment",
            type: "POST",
            data: {ID: id, Content: content },
            success: function () {
                $(`#comment-content-${id}`).text(content);
                form.addClass("d-none");
                    $(`#comment-text-${id}`).show();
                    },
            error: function () {
                alert("Error updating comment");
                    }
                });
    });
    $(document).on("click", ".delete-comment-btn", function () {
        let id = $(this).data("id");

    $.ajax({
        url: "/Comment/DeleteComment",
        type: "POST",
        data: {Id: id, DeletedBy: currentUser },
        success: function (res) {
                    if (res.success) {
                        $(`#comment-${id}`).remove();
                                } else {
                        alert(res.message || "Error deleting comment");
                                }
                    },
        error: function () {
            alert("Server error while deleting comment");
                }
            });
    });


    $(document).ready(function () {
        @foreach(var post in Model)
    {
        <text>loadComments(@post.ID);</text>
    }
    });
</script>



