$(document).ready(function () {

    $("#btnLogin").click(function () {
        var username = $('#username').val();
        var password = $('#password').val();
        if (username && password) {
            var payload = { username: username, password: password };
            $.ajax({
                type: 'POST',
                url: '/Home/ValidateLogin/',
                data: payload,
                success: function (result) {
                    if (result) {
                        document.getElementById('AuthorId').value = result.id;
                        document.getElementById('login').hidden = true;
                        document.getElementById('DivLoggedUser').hidden = false;
                        if (result.roleId == 1) {
                            document.getElementById('DivWriterUser').hidden = false;
                            loadWriterGrid();
                        }
                        else {
                            document.getElementById('DivEditorUser').hidden = false;
                            loadEditorGrid();
                        }
                    }
                    else {
                        alert('Wrong user or password');
                    }
                }
            });
        }
        else {
            alert('Please enter the information');
        }
    });

    $("#btnAnonimous").click(function () {
        $.ajax({
            type: 'GET',
            url: '/Home/AnominousAccess/',
            success: function (result) {
                paintGrid(result);
            }
        });
    });

    $("#btnNew").click(function () {
        cleanForm();
    });

    $("#btnSave").click(function () {
        var blogId = $('#blogIdnew').val();
        var blogTitle = $('#blogTitle').val();
        var blogContent = $('#blogContent').val();
        var authorId = document.getElementById('AuthorId').value;
        if (blogTitle && blogContent) {
            var payload = { blogId: blogId, blogTitle: blogTitle, blogContent: blogContent, authorId: authorId, statusId:'5' };
            $.ajax({
                type: 'POST',
                url: '/Home/Save/',
                data: payload,
                success: function (result) {
                    if (result) {
                        loadWriterGrid();
                        cleanForm();
                    }
                    else {
                        alert('An error has ocurred');
                    }
                }
            });
        }
        else {
            alert('Please enter the information');
        }
    });

    function cleanForm() {
        document.getElementById('blogIdnew').value = '';
        document.getElementById('blogTitle').value = '';
        document.getElementById('blogContent').value = '';
    }

    function loadWriterGrid() {
        var authorId = document.getElementById('AuthorId').value;
        $.ajax({
            type: 'GET',
            data: "authorId=" + authorId,
            url: '/Home/GetBlogsByAutor/',
            success: function (result) {
                paintGrid(result, "writer");
            }
        });
    }

    function loadEditorGrid() {
        $.ajax({
            type: 'GET',
            url: '/Home/GetBlogsPendingToApproval/',
            success: function (result) {
                paintGrid(result, "Editor");
            }
        });
    }

    function deleteBlog(blogId) {
        var authorId = document.getElementById('AuthorId').value;
        var payload = { blogId: blogId, authorId: authorId};
        $.ajax({
            type: 'DELETE',
            data: payload,
            url: '/Home/DeleteBlog/',
            success: function (result) {
                loadEditorGrid();
            }
        });
    }

    function saveBlogApproval(blogId, approve) {
        var authorId = document.getElementById('AuthorId').value;
        var payload = { blogId: blogId, authorId: authorId, approve: approve };
        $.ajax({
            type: 'POST',
            data: payload,
            url: '/Home/SaveBlogApproval/',
            success: function (result) {
                loadEditorGrid();
            }
        });
    }

    function paintGrid(result, user) {
        let ratesTable = '<div id="listBlogs">';

        ratesTable += '<div class="row">';
        if (user == "Editor") {
            ratesTable += '<div class="form-group col-md-1 "></div>';
            ratesTable += '<div class="form-group col-md-1 "></div>';
            ratesTable += '<div class="form-group col-md-1"></div>';
        }
        if (user == "writer") {
            ratesTable += '<div class="form-group col-md-1"></div>';
        }
        ratesTable += '<div class="form-group col-md-1"></div>';
        ratesTable += '<div class="form-group col-md-1">Blog Id</div>';
        ratesTable += '<div class="form-group col-md-1">Title</div>';
        ratesTable += '<div class="form-group col-md-2">Content</div>';
        ratesTable += '<div class="form-group col-md-1">Status</div>';
        ratesTable += '<div class="form-group col-md-2">AuthorFullName</div>';
        ratesTable += '<div class="form-group col-md-2">SubmitDate</div>';
        ratesTable += '</div>';

        Object.keys(result).forEach(function (key) {
            ratesTable += '<div id=' + result[key].blogId + ' class="row">';
            if (user == "Editor") {
                ratesTable += '<div class="form-group col-md-1"><i class="fas fa-trash pointer" id=' + result[key].blogId + '></i></div>';
                ratesTable += '<div class="form-group col-md-1"><i class="fas fa-thumbs-up pointer" id=' + result[key].blogId + '></i></div>';
                ratesTable += '<div class="form-group col-md-1"><i class="fas fa-thumbs-down pointer" id=' + result[key].blogId + '></i></div>';
            }
            if (user == "writer") {
                ratesTable += '<div class="form-group col-md-1"><i class="fas fa-pen pointer" id=' + result[key].blogId + '></i></div>';
            }
            ratesTable += '<div class="form-group col-md-1"><i class="fas fa-cloud pointer" id=' + result[key].blogId + '></i></div>';
            ratesTable += '<div class="form-group col-md-1">' + result[key].blogId + '</div>';
            ratesTable += '<div class="form-group col-md-1" id=title' + result[key].blogId + '>' + result[key].title + '</div>';
            ratesTable += '<div class="form-group col-md-2" id=content' + result[key].blogId + '>' + result[key].content + '</div>';
            ratesTable += '<div class="form-group col-md-1">' + result[key].statusDescription + '</div>';
            ratesTable += '<div class="form-group col-md-2">' + result[key].authorFullName + '</div>';
            ratesTable += '<div class="form-group col-md-2">' + result[key].submitDate + '</div>';
            ratesTable += '</div>';
            console.log(key, result[key].BlogId)
        })
        document.getElementById('GridAnonimous').innerHTML = ratesTable;
        document.getElementById('DivAnonimous').hidden = false;
        document.getElementById('login').hidden = true;

        $(".fa-cloud").click(function () {
            var id = document.getElementById('blogId');
            id.value = this.id;
        });

        $(".fa-pen").click(function () {
            document.getElementById('blogIdnew').value = this.id;
            document.getElementById('blogTitle').value = document.getElementById('title' + this.id).textContent;
            document.getElementById('blogContent').value = document.getElementById('content' + this.id).textContent;
        });

        $(".fa-trash").click(function () {
            deleteBlog(this.id);
        });

        $(".fa-thumbs-up").click(function () {
            saveBlogApproval(this.id, true);
        });

        $(".fa-thumbs-down").click(function () {
            saveBlogApproval(this.id, false);
        });

        $("#btnSubmitComment").click(function () {
            var id = document.getElementById('blogId').value;
            var comment = document.getElementById('blogComment').value;

            if (id && comment) {
                //Todo Save Comment
            }
            else {
                alert('Please fill the information before save the comment');
            }
        });
    }
});