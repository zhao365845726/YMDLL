/*************************** --公共验证使用的js Start-- *****************************/
//手机号码验证
function validatemobile(mobile) {
    if (mobile.length == 0) {
        alert('请输入手机号码！');
        return false;
    }
    if (mobile.length != 11) {
        alert('请输入有效的手机号码！');
        return false;
    }
    //可验证153（联通）,159（移动）等新区段的开通
    var myreg = /^(((13[0-9]{1})|159|153)+\d{8})$/;
    if (!myreg.test(mobile)) {
        alert('请输入有效的手机号码！');
        return false;
    }
}

//电子邮箱的验证
function validateemail(email) {
    //对电子邮件的验证
    var myreg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
    if (!myreg.test(email)) {
        alert('提示\n\n请输入有效的E_mail！');
        return false;
    }
}

//验证密码
function validatepassword(pwd) {
    var test = /^[a-zA-Z0-9]+$/;              //正则表达式
    if (test.exec(pwd))                            //exec(string) 为验证字符串是否符合正则表达式
    {
        alert("对不起,只能输入8-16含有特殊符号字母数字的密码");
        return false;
    }
}

//js获取URL中的参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}

function GetRequestString() {
    var urlinfo = window.location.href;//获取url
    alert(urlinfo);
    var userName = urlinfo.split("?")[1].split("=")[1];//拆分url得到”=”后面的参数
    return decodeURI(userName);
}
/*************************** --公共验证使用的js End-- *****************************/