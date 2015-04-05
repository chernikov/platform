function TeamPlayerInfo()
{
    var _this = this;

    this.init = function ()
    {
        $("#playerTabs").tab();
    }
}

var teamPlayerInfo = null;
$(function () {
    teamPlayerInfo = new TeamPlayerInfo();
    teamPlayerInfo.init();
});