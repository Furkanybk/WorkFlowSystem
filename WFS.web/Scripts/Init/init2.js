$(document).ready(function () {

    var notifhub = $.connection.notifHub;

    // Show first notification
    notifhub.client.showMessage = function (message, action) {
        var actionName = action,
            toastMessage1 = {
                text: message + '-' + actionName,
                sticky: false,
                position: 'top-right',
                type: 'success',
                closeText: ''
            };
        var Toast1 = $().toastmessage('showToast', toastMessage1); // display notification
    };

    //  show database notifiction status
    notifhub.client.addMessage = function (jsonTickets , uId) {   

        var TicketDetails = []; 

        var obj = $.parseJSON(jsonTickets);

        // assign ticketdetails from server to TicketDetails array
        TicketDetails = obj.NotifDetailsList; 
        var count = 0; 
        // build up table row from array TicketsDetailsList
        if (TicketDetails !== undefined && TicketDetails.length !== 0) {   
            document.getElementById('notfArea').innerHTML = "";
            $.each(TicketDetails, function () {    
                if (count < 5) {   
                    notif = "<a href='" + this['DetailUrl'] + "' class='dropdown-item'><div class='icon'> <svg xmlns='http://www.w3.org/2000/svg' width='24' height='24' viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='2' stroke-linecap='round' stroke-linejoin='round' class='feather feather-layers'><polygon points='12 2 2 7 12 12 22 7 12 2'></polygon><polyline points='2 17 12 22 22 17'></polyline><polyline points='2 12 12 17 22 12'></polyline></svg></div><div class='content'><p>" + this['Title'] + "</p><p class='sub-text text-muted'>" + timeConvert(this['CreateDate']) + "</p></div></a>";
                    $("#notfArea").append(notif);
                }
                else {
                    return false;
                }
                if (!this['NotifStatus']) {
                    $.ajax({
                        url: "/Notification/Notify",
                        type: "POST",
                        data: { notifId: this['NotificationId'] }, 
                        success: function (data) { 
                        }
                    });  
                    showToast("success", this['Title']);
                }  
                count++; 
            });
            document.getElementById("notifTitle").innerHTML = "Bildirimler (" + TicketDetails.length + ")";
            document.getElementById("belll").hidden = false;
            //console.log(TicketDetails);
            //var Toast1 = $().toastmessage('showToast', toastMessage2); // display notification 
        } 

        else { 
            document.getElementById('notfArea').innerHTML = "";
            document.getElementById("notifTitle").innerHTML = "Bildirimler (" + TicketDetails.length + ")";
            document.getElementById("belll").hidden = true;
            $("#notfArea").append("<a href='javascript:;' class='dropdown-item'><div class='content' style = 'text-align:center;' ><p>Yeni Bildirim Yok</p></div></a>");  
        } 
    };

    // start SignalR    
    $.connection.hub.start().done(function () {
        //notifhub.server.send('Start', 'Hub started'); // call (server) function send() in TicketsHub.cs
        notifhub.server.start(); // call (server) function start() in TicketsHub.cs 
    });

});

function timeConvert(Date) {
     
    var res = Date.split(".");  
    var day = res[0]; 
    var res2 = res[2].split(" ");  
    var res3 = res2[1].split(":"); 
    var hour = res3[0];
    var min = res3[1];
     
    var currentDay = moment().format("DD");   
    var currentHour = moment().format("HH");   
    var currentMin = moment().format("mm");   
     

    day = Math.abs(day - currentDay);

    hour = Math.abs(hour - currentHour);
     
    min = 60 - min + +currentMin; 

    if (min < 60) { 
        return min + " dk önce";
    }
    else if (hour < 24 && hour !== 0){
        return hour + " saat önce";
    }
    else if (day >= 1) { 
        return day + " gün önce"; 
    }
    else { 
        return "az önce"; 
    }
} 