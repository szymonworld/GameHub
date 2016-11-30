<?php

class DB_CONNECT {

    function __construct() 
	{
        $this->connect();
    }
 
    function __destruct() 
	{
        $this->close();
    }
 
    function connect() 
	{
		try
		{
			require_once __DIR__ . '/db_config.php';
			$pdo = new PDO('mysql:host='.DB_SERVER.';dbname='.DB_DATABASE.';port='.DB_PORT, DB_USER, DB_PASSWORD );

		}
		catch(PDOException $e)
		{
			return 0;
		}
        
        return $pdo;
    }
 
}
 
?>