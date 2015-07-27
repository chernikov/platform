function UserList() {
    _this = this;

    this.init = function ()
    {
        $('#confirmDeleteModal').bind('show', function () {
            var id = $(this).data('id'),
                removeBtn = $(this).find('.btn-danger'),
                href = removeBtn.attr('href');

            removeBtn.attr('href', href.replace(/\?id=\d*/, '?id=' + id));
        });

        $(".confirm-delete").click(function (e) {
            e.stopPropagation();

            var id = $(this).data('id');
            $('#confirmDeleteModal').data('id', id).modal('show');
        });

        $(".loginAs").click(function (e) {
            e.stopPropagation();
            var id = $(this).data('id');
            window.location = "/admin/User/Login/" + id;
        });

        $(".startTutorial").click(function (e) {
            var id = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/admin/User/StartTutorial/" + id,
                success: function (data) {
                    alert("User start tutorial");
                }
            })
            e.stopPropagation();
        });
        
    }
}

var userList = null;

$().ready(function () {
    userList = new UserList();

    userList.init();
});
