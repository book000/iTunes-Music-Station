/* Firefox専用通知の奴 */
function notifyReq(){
if (navigator.userAgent.indexOf("Firefox") > -1) {
  Notification.requestPermission(function(permission){
    console.debug("Notification permission: "+permission);
    if(Notification.permission == "granted"){
      //notify("notifyReq後");
    }
    });
    }else{
    // 表示可能かの権限チェックを行います
   var hasPermission = window.webkitNotifications.checkPermission();
    if (hasPermission !== 0) {
    // 権限が無い場合には、ユーザーへ要求します。
    window.webkitNotifications.requestPermission();
    return;
}
}
};
function notify(body){
if (navigator.userAgent.indexOf("Firefox") > -1) {
  switch(Notification.permission){
    case "granted":
      new Notification("iTunes Music Station", {
        icon:"favicon.ico",
        body:body,
        tag:body,
      });
      break;
    case "default":
      notifyReq();
      break;
    case "denied":
      console.warn("デスクトップ通知が拒否されています");
      break;
  }
}else{
  window.notification = window.webkitNotifications.createNotification(
    "favicon.ico",
    "iTunes Music Station",
    body
);
notification.show();
}
};
