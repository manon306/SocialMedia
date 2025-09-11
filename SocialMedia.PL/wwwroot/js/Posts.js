@model List < PostVm >
    $(document).ready(function () {
        // تحميل معلومات التفاعلات لكل منشور
        @foreach(var post in Model)
    {
        <text>
            loadReactsInfo(@post.ID);
        </text>
    }

            // إضافة تفاعل جديد
    $(document).on('click', '.react-option', function() {
                var postId = $(this).closest('.react-btn-group').data('postid');
    var reactType = $(this).data('type');

    toggleReact(postId, reactType);
            });

    // إزالة التفاعل عند الضغط على الزر الرئيسي
    $(document).on('click', '.react-main-btn', function() {
                var postId = $(this).closest('.react-btn-group').data('postid');
    var currentReact = $(this).find('.react-icon').data('current-react');

    if (currentReact) {
        // إذا كان هناك تفاعل موجود، إزالته
        toggleReact(postId, currentReact);
                } else {
        // إذا لم يكن هناك تفاعل، إضافة Like افتراضي
        toggleReact(postId, @((int)reactType.Like));
                }
            });
        });

    function toggleReact(postId, reactType) {
        $.ajax({
            url: '/React/ToggleReact',
            type: 'POST',
            data: {
                postId: postId,
                type: reactType
            },
            success: function (response) {
                if (response.success) {
                    updateReactUI(postId, response);
                } else {
                    showAlert('Error', response.message, 'danger');
                }
            },
            error: function () {
                showAlert('Error', 'An error occurred while reacting', 'danger');
            }
        });
        }

    function loadReactsInfo(postId) {
        $.ajax({
            url: '/React/GetReactsInfo',
            type: 'GET',
            data: { postId: postId },
            success: function (response) {
                if (response.success) {
                    updateReactUI(postId, response);
                }
            },
            error: function () {
                console.log('Error loading reacts info for post ' + postId);
            }
        });
        }

    function updateReactUI(postId, data) {
        // تحديث العدد الإجمالي
        $('.reacts-count[data-postid="' + postId + '"]').text(data.count);

    // تحديث الزر الرئيسي بناءً على تفاعل المستخدم
    var reactBtn = $('.react-btn-group[data-postid="' + postId + '"] .react-main-btn');
    var reactIcon = reactBtn.find('.react-icon');
    var reactText = reactBtn.find('.react-text');

    if (data.userReactType !== null) {
        // إذا كان المستخدم متفاعل
        reactBtn.addClass('active');

    // تحديث الأيقونة والنص بناءً على نوع التفاعل
    switch (data.userReactType) {
                    case @((int)reactType.Love):
    reactIcon.removeClass().addClass('bi bi-heart-fill text-danger');
    reactText.text('Love');
    break;
    case @((int)reactType.Like):
    reactIcon.removeClass().addClass('bi bi-hand-thumbs-up-fill text-primary');
    reactText.text('Like');
    break;
    case @((int)reactType.Care):
    reactIcon.removeClass().addClass('bi bi-emoji-heart-eyes-fill text-warning');
    reactText.text('Care');
    break;
    case @((int)reactType.angry):
    reactIcon.removeClass().addClass('bi bi-emoji-angry-fill text-danger');
    reactText.text('Angry');
    break;
                }

    // حفظ نوع التفاعل الحالي
    reactIcon.data('current-react', data.userReactType);
            } else {
        // إذا لم يكن المستخدم متفاعل
        reactBtn.removeClass('active');
    reactIcon.removeClass().addClass('bi bi-heart');
    reactText.text('Like');
    reactIcon.data('current-react', null);
            }

    // تحديث الملخص (يمكنك إضافة هذا لاحقاً إذا أردت عرض تفاصيل أكثر)
    console.log('React summary for post ' + postId + ':', data.summary);
        }

    function showAlert(title, message, type) {
            // دالة لعرض التنبيهات
            var alertClass = 'alert-' + type;
    var alertHtml = `
    <div class="alert ${alertClass} alert-dismissible fade show" role="alert">
        <strong>${title}</strong> ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </div>
    `;

    // إضافة التنبيه في أعلى الصفحة
    $('.container.py-4').prepend(alertHtml);

    // إزالة التنبيه تلقائياً بعد 3 ثواني
    setTimeout(function() {
        $('.alert').alert('close');
            }, 3000);
        }
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
                                    <div class="d-flex mb-3 comment-item" id="comment-${c.id}">
        <div class="flex-shrink-0">
            <img src="https://via.placeholder.com/40" class="rounded-circle me-2" width="40" height="40" alt="Profile">
        </div>
        <div class="flex-grow-1 ms-2">
            <div class="comment-content p-3 rounded" id="comment-text-${c.id}">
                <div class="d-flex justify-content-between align-items-start mb-1">
                    <strong class="d-block text-white">${currentUser}</strong>
                    <span class="text-muted small">Just now</span>
                </div>
                <p class="mb-0 text-white" id="comment-content-${c.id}">${c.content}</p>
            </div>

            <!-- فورم التعديل -->
            <form class="edit-comment-form d-none mt-2" data-id="${c.id}">
                <input type="hidden" name="ID" value="${c.id}" />
                <input type="text" name="Content" class="form-control mb-2 bg-secondary text-white border-0" value="${c.content}" />
                <div class="d-flex">
                    <button type="submit" class="btn btn-sm btn-accent me-2">Save</button>
                    <button type="button" class="btn btn-sm btn-outline-light cancel-edit" data-id="${c.id}">Cancel</button>
                </div>
            </form>

            <div class="comment-actions mt-2">
                <button class="btn btn-sm btn-link text-muted p-0 me-3 text-decoration-none">Like</button>
                <button class="btn btn-sm btn-link text-muted p-0 me-3 text-decoration-none reply-btn" data-id="${c.id}">Reply</button>
                <button class="btn btn-sm btn-link text-muted p-0 me-3 text-decoration-none edit-btn" data-id="${c.id}">Edit</button>
                <button type="button" class="btn btn-sm btn-link text-danger p-0 delete-comment-btn" data-id="${c.id}">
                    <i class="bi bi-trash me-1"></i> Delete
                </button>
            </div>

            <!-- الردود -->
            <div class="replies mt-2 ms-3 ps-3 border-start border-secondary" id="replies-${c.id}">
                <!-- سيتم إضافة الردود هنا -->
            </div>

            <!-- نموذج الرد (مخفي افتراضيًا) -->
            <form class="reply-form d-none mt-2" data-commentid="${c.id}">
                <div class="d-flex align-items-center">
                    <img src="https://via.placeholder.com/32" class="rounded-circle me-2" width="32" height="32" alt="Profile">
                    <input type="hidden" name="ParentCommentID" value="${c.id}" />
                    <input type="text" name="Content" class="form-control form-control-sm rounded-pill bg-secondary text-white border-0 me-2" placeholder="Write a reply..." />
                    <button type="submit" class="btn btn-sm btn-accent rounded-circle d-flex align-items-center justify-content-center" style="width: 32px; height: 32px;">
                        <i class="bi bi-send"></i>
                    </button>
                    <button type="button" class="btn btn-sm btn-outline-light ms-2 cancel-reply" data-commentid="${c.id}">Cancel</button>
                </div>
            </form>
        </div>
    </div>
                    `);
                        loadReplies(c.id);
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
    // إرسال رد
    $(document).on("submit", ".reply-form", function (e) {
        e.preventDefault();

    let form = $(this);
    let commentId = form.find("input[name='ParentCommentID']").val();
    let content = form.find("input[name='Content']").val();

    $.ajax({
        url: "/Reply/AddComment",   // زي ما عاملتي في الـ Controller
    type: "POST",
    data: {ParentCommentID: commentId, Content: content },
    success: function (res) {
                if (res.success) {
        // ضيفي الرد في منطقة replies
        $(`#replies-${commentId}`).append(`
                        <div class="reply bg-light p-2 rounded mb-1">
                            <strong>${currentUser}</strong>: ${content}

                        </div>
                    `);
    form.find("input[name='Content']").val(""); // امسحي التكست
                } else {
        alert(res.message || "Error adding reply");
                }
            },
    error: function () {
        alert("Server error while adding reply");
            }
        });
    });

    function loadReplies(commentId) {
        $.ajax({
            url: `/Reply/GetAllComments?id=${commentId}`, // نفس الشكل عندك
            type: "GET",
            success: function (data) {
                let container = $(`#replies-${commentId}`);
                container.find(".reply").remove(); // امسح القديم
                if (data && data.length > 0) {
                    data.forEach(r => {
                        container.append(`
                            <div class="reply bg-light p-2 rounded mb-1">
                                <strong>${currentUser}</strong>: ${r.content}
                            </div>
                        `);
                    });
                }
            },
            error: function () {
                console.log("Error loading replies.");
            }
        });
    }

    // إظهار فورم المشاركة عند الضغط على Share
    $(document).on("click", ".share-btn", function() {
        var postId = $(this).data("postid");
    $(this).closest(".card-body").find(".share-form").removeClass("d-none");
    });

    // إلغاء المشاركة
    $(document).on("click", ".cancel-share", function() {
        $(this).closest(".share-form").addClass("d-none");
    });
    // إظهار نموذج الرد عند الضغط على Reply
    $(document).on("click", ".reply-btn", function () {
        let id = $(this).data("id");
    $(`form[data-commentid='${id}']`).removeClass("d-none");
    $(this).addClass("d-none");
    });

    // إخفاء نموذج الرد عند الضغط على Cancel
    $(document).on("click", ".cancel-reply", function () {
        let id = $(this).data("commentid");
    $(`form[data-commentid='${id}']`).addClass("d-none");
    $(`button.reply-btn[data-id='${id}']`).removeClass("d-none");
    });

    $(document).ready(function () {
        @foreach(var post in Model)
    {
        <text>loadComments(@post.ID);</text>
    }
    });



