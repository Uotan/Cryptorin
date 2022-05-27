<?php
    //all time zones
    //https://www.php.net/manual/ru/timezones.php
    function GetTime()
    {
        date_default_timezone_set("Asia/Krasnoyarsk");
        $date = date('Y-m-d H:i:s', time());
        return $date;
    }
?>