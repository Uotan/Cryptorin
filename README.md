
# Project Cryptorin
## Getting started instructions

### Server preparation
To get started, you need a web server. Copy the contents of the [Domain](https://github.com/Uotan/Cryptorin/tree/master/Domain) folder to the root of the domain.

You can change the start logo in the app. Replace the `Domain/logo.png` file.

Restore MySQL database from `Domain/sql_script.sql` file.

Edit the 'Domain/connect.php` file. Insert your data in the appropriate fields.
```php
<?php 
    $host = '127.0.0.1';
    $db   = 'your_DB_name';
    $user = 'your_user';
    $pass = 'your_password';
    $charset = 'utf8';
    $dsn = "mysql:host=$host;dbname=$db;charset=$charset";
    $opt = [
        PDO::ATTR_ERRMODE            => PDO::ERRMODE_EXCEPTION,
        PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
        PDO::ATTR_EMULATE_PREPARES   => false,
    ];
    $pdo = new PDO($dsn, $user, $pass, $opt);
?>
```
Edit the 'Domain/getHash.php` file. Create your salt with a length of more than 22 characters (optional, but desirable).
```php
<?php
    function GetHash($secretString)
    {
        $options = ['salt' => 'your_salt','cost' => 12];
        $hash = password_hash($secretString, PASSWORD_DEFAULT, $options);
        return $hash;
    }
?>
```
Edit the 'Domain/getTime.php` file. Set a time zone that is convenient for you. [All time zones](https://www.php.net/manual/ru/timezones.php)
```php
<?php
    function GetTime()
    {
        date_default_timezone_set("Asia/Krasnoyarsk");
        $date = date('Y-m-d h:i:s', time());
        return $date;
    }
?>
```
### Preparing a mobile application
After installing the application on the authorization page, click the "Change domain" button located in the upper right corner. The domain format should be like this : `https://yourdomain.com`

## Work instructions
After successful authorization, you can start using. To start communicating with another user, you need to exchange your identification numbers. Click on the "+" button to find the user by ID.
Be sure to visit the settings menu.