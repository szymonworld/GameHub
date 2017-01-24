<?php
 
require_once __DIR__ . '/db_connect.php';
//error_reporting(E_ERROR | E_PARSE);

$selectedType = null;
$conn = null;

if(checkType())
{
	parseType();
}

function checkType()
{
	global $conn;
	for ($i = 0; $i < 18; $i++) 
	{
		if($_GET['type'] == $i)
		{
			global $selectedType;
			$selectedType = $i; 
			
			try 
			{
				$db = new DB_CONNECT();
				$conn = $db->connect();
			} 
			catch(PDOException $e)
			{
				jsonResponse(0, 'Connection failed'.$e->getMessage());
				return false;
			}
			
			$conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
			return true;
		} 
	}
	jsonResponse(0, "Cannot recognize request type.");
	return false;
}

function parseType()
{
	global $selectedType;
	
	switch ($selectedType) 
	{
		case 0:
			if(isset($_GET['email']) and isset($_GET['password']))
			{
				createAccount($_GET['login'], $_GET['password'], $_GET['firstname'], $_GET['lastname'], $_GET['email'], $_GET['mic'], $_GET['pic'], $_GET['status'], $_GET['lang'], $_GET['rep'],$_GET['psn'], $_GET['xbox'], $_GET['steam'], $_GET['origin'], $_GET['discord'], $_GET['uplay'], $_GET['battle'], $_GET['lol'], $_GET['skype']);
			}
			else
			{
				jsonResponse(0,"Invalid parameters.");
			}
			break;
		case 1:
			getAccountByEmail($_GET['email']);
			break;
		case 2:
			getAllEvents($_GET['email'], $_GET['password']); 
			break;
		case 3:
			getEvent($_GET['email'], $_GET['password'], $_GET['eventid']);
			break;
		case 4:
			addEvent($_GET['email'], $_GET['password'], $_GET['eventName'], $_GET['gameid'], $_GET['description'], $_GET['date'], $_GET['hour'], $_GET['timespan'], $_GET['public'], $_GET['platform'], $_GET['slots'], $_GET['microphone']);
			break;
		case 5:
			getEventParticipants($_GET['email'], $_GET['password'], $_GET['eventid']);
			break;
		case 6:
			addEventParticipant($_GET['email'], $_GET['password'], $_GET['eventid'], $_GET['accountid']);
			break;
		case 7:
			getFriends($_GET['email'], $_GET['password']);
			break;
		case 8:
			addFriend($_GET['email'], $_GET['password'], $_GET['accountid']);
			break;
		case 9:
			deleteFriend($_GET['email'], $_GET['password'], $_GET['accountid']);
			break;
		case 10:
			getInvitations($_GET['email'], $_GET['password']);
			break;
		case 11:
			addInvitation($_GET['email'], $_GET['password'], $_GET['accountid'], $_GET['eventid']);
			break;
		case 12:
			deleteInvitation($_GET['email'], $_GET['password'], $_GET['inviteid']);
			break;
		case 13:
			login($_GET['email'], $_GET['password']);
			break;
		case 14:
			isAccountExist($_GET['email']);
			break;	
		case 15:
			getHashByEmail($_GET['email']);
			break;	
		case 16:
			getLinkAccounts($_GET['email'], $_GET['password']);
			break;
		case 17:
			updateAccount($_GET['email'], $_GET['password'], $_GET['c'], $_GET['v']);
			break;
		default:
			break;
		
	}
}
 
 
function authenticate($email, $password)
{
	//return password_verify($password, loadHashByLogin($login));
	if(strlen($password) > 1)
	{
		return password_verify($password, loadHashByEmail($email));
		//return $password == loadHashByEmail($email);
	}
	else
	{
		jsonResponse(0,"Invalid password.");
	}
}
 
function loadHashByEmail($email)
{
	global $conn;
	$query = $conn->prepare("SELECT Password FROM Accounts WHERE Email = :email");
	$query->bindValue(':email', $email);
	$query->execute();
	$res = $query->fetch();
	return $res['Password'];
}

function isEmailExist($email)
{
	global $conn;
	$query = $conn->prepare("SELECT count(*) FROM Accounts WHERE Email = :email"); 
	$query->bindValue(":email",$email);
	$query->execute(); 
	return $query->fetchColumn() > 0; 	
}

function jsonResponse($success, $message)
{
	$response = array();
	$response["success"] = $success;
	$response["message"] = $message;	
	echo json_encode($response);	
}

function createEvent($email, $password, $eventName, $description, $date, $game)	 
{
	if(authenticate($email, $password))
	{	
		try 
		{  
			global $conn;
			$conn->beginTransaction();
			
			$query = $conn->prepare("INSERT INTO Events(EventName, Description, Date, GameID) VALUES(:eventName, :description, :date, :game)");
			$query->bindValue(':eventName', $eventName);
			$query->bindValue(':description', $description);
			$query->bindValue(':date', $date);
			$query->bindValue(':game', $game);
			$query->execute();
			
			$conn->commit();
			
			jsonResponse(1, "Event created.");	
	  
		} 
		catch (Exception $e) 
		{
			$conn->rollBack();
			jsonResponse(0, "Error.");
		}
			
	}
	else
	{
		jsonResponse(0, "Authentification error.");	
	}
	
}

function getEvent($email, $password, $eventID)
{
	if(authenticate($email, $password))
	{	
		global $conn;
		$query = $conn->prepare("SELECT * FROM Events WHERE EventID = :eventID"); 
		$query->bindValue(":eventID",$eventID);
		$query->execute(); 		
		$fetch = $query->fetch();
		
		$response = array();
		$response['EventName'] = $fetch['EventName'];
		$response['GameID'] = $fetch['GameID'];
		$response['Description'] = $fetch['Description'];
		$response['Datee'] = $fetch['Datee'];
		$response['Hourr'] = $fetch['Hourr'];
		$response['Timespan'] = $fetch['Timespan'];
		$response['Public'] = $fetch['Public'];
		$response['Platform'] = $fetch['Platform'];
		$response['Slots'] = $fetch['Slots'];
		$response['Microphone'] =  $fetch['Microphone'];
		$response['success'] = 1;
		
		echo json_encode($response);
	}
	else
	{
		jsonResponse(0, "Error.");
	}
	
}

function getAllEvents($email, $password)
{
	if(authenticate($email, $password))
	{	
		global $conn;

		$query = $conn->prepare("SELECT * FROM Events"); 
		$query->execute(); 		
		$response = array();
		$response['events'] = array();

		while($fetch = $query->fetch(PDO::FETCH_ASSOC))
		{
			$event = array();
			$event['EventID'] = $fetch['EventID'];
			$event['EventName'] = $fetch['EventName'];
			$event['GameID'] = $fetch['GameID'];
			$event['Description'] = $fetch['Description'];
			$event['Datee'] = $fetch['Datee'];
			$event['Hourr'] = $fetch['Hourr'];
			$event['Timespan'] = $fetch['Timespan'];
			$event['Public'] = $fetch['Public'];
			$event['Platform'] = $fetch['Platform'];
			$event['Slots'] = $fetch['Slots'];
			$event['Microphone'] =  $fetch['Microphone'];
			array_push($response['events'], $event);
		}	

		$response['success'] = 1;
		echo safe_json_encode($response);
 
	}
	else
	{
		jsonResponse(0, "Error.");
	}
	
}
function addEvent($email, $password, $eventName, $gameID, $description, $date, $hour, $timespan, $public, $platform, $slots, $microphone)
{
	if(authenticate($email, $password))
	{	
		
		try 
		{  
			global $conn;		
			$conn->beginTransaction();
			
			$query = $conn->prepare("INSERT INTO Events(EventName, GameID, Description, Datee, Hourr, Timespan, Public, Platform, Slots, Microphone) 
									 VALUES(:eventName, :gameID, :description, :date, :hour, :timespan, :public, :platform, :slots, :microphone)");
									 
			$query->bindValue(':eventName', $eventName);
			$query->bindValue(':gameID', $gameID);
			$query->bindValue(':description', $description);
			$query->bindValue(':date', $date);
			$query->bindValue(':hour', $hour);
			$query->bindValue(':timespan', $timespan);
			$query->bindValue(':public', $public);
			$query->bindValue(':platform', $platform);
			$query->bindValue(':slots', $slots);
			$query->bindValue(':microphone', $microphone);
			$query->execute();
			
			$conn->commit();
			
			jsonResponse(1, "Event created.");
			
		}
		catch (Exception $e) 																																																																								
		{
			$conn->rollBack();
			jsonResponse(0, "Error.");
		}
	}
	else
	{
		jsonResponse(0, "Error.");
	}
}

function getEventParticipants($email, $password, $eventID)
{
	if(authenticate($email, $password))
	{	
		global $conn;
		$query = $conn->prepare("SELECT * FROM EventParticipants WHERE EventID = :eventID"); 
		$query->bindValue(":eventID",$eventID);
		$query->execute(); 		
		$response = array();
		$response['events'] = array();
		
		while($fetch = $query->fetch())
		{
			$event = array();
			$event['ParticipationID'] = $fetch['ParticipationID'];
			$event['AccountID'] = $fetch['AccountID'];
			array_push($response['events'], $event);
			
		}	
		echo json_encode($response);
	}	
	else
	{
		jsonResponse(0, "Error.");
	}
}

function addEventParticipant($email, $password, $eventID, $accountID)
{
	if(authenticate($email, $password))
	{	
		
		try 
		{  
			global $conn;		
			$conn->beginTransaction();
			
			$query = $conn->prepare("INSERT INTO EventParticipants(EventID, AccountID) 
									 VALUES(:eventID, :accountID)");
									 
			$query->bindValue(':eventID', $eventID);
			$query->bindValue(':accountID', $accountID);
			$query->execute();
			
			$conn->commit();
			
			jsonResponse(1, "Event participant added.");
			
		}
		catch (Exception $e) 																																																																								
		{
			$conn->rollBack();
			jsonResponse(0, "Error.");
		}
	}
	else
	{
		jsonResponse(0, "Error.");
	}
}

function getFriends($email, $password)
{
	if(authenticate($email, $password))
	{	
		global $conn;
		
		$query = $conn->prepare("SELECT AccountID FROM Accounts WHERE Email = :email");
		$query->bindValue(':email', $email);
		$query->execute();
		$accountID = $query->fetch();
		
		$query = $conn->prepare("SELECT * FROM Friends WHERE AccountID1 = :accountID"); 
		$query->bindValue(":accountID",$accountID['AccountID']);
		$query->execute(); 		
		$response = array();
		$response['friends'] = array();
		
		while($fetch = $query->fetch())
		{
			$friend = array();
			$friend['FriendshipID'] = $fetch['FriendshipID'];
			$friend['AccountID2'] = $fetch['AccountID2'];
			array_push($response['friends'], $friend);
			
		}	
		
		echo json_encode($response);
	}	
	else
	{
		jsonResponse(0, "Error.");
	}
}

function addFriend($email, $password, $AccountID2)
{
	if(authenticate($email, $password))
	{	
		global $conn;
		
		$query = $conn->prepare("SELECT AccountID FROM Accounts WHERE Email = :email");
		$query->bindValue(':email', $Email);
		$query->execute();
		$accountID = $query->fetch();
		
		try 
		{  	
			$conn->beginTransaction();
			
			$query = $conn->prepare("INSERT INTO Friends(AccountID1, AccountID2) 
									 VALUES(:accountID1, :accountID2)");
									 
			$query->bindValue(':accountID1', $accountID['AccountID']);
			$query->bindValue(':accountID2', $accountID2);
			$query->execute();
			
			$conn->commit();
			
			jsonResponse(1, "Friend added.");
			
		}
		catch (Exception $e) 																																																																								
		{
			$conn->rollBack();
			jsonResponse(0, "Error.");
		}
	}
	else
	{
		jsonResponse(0, "Error.");
	}
}

function deleteFriend($email, $password, $AccountID2)
{
	if(authenticate($email, $password))
	{	
		global $conn;
		
		$query = $conn->prepare("SELECT AccountID FROM Accounts WHERE Email = :email");
		$query->bindValue(':email', $email);
		$query->execute();
		$accountID = $query->fetch();
		
		try 
		{  	
			$conn->beginTransaction();
			
			$query = $conn->prepare("DELETE FROM Friends WHERE AccountID1 = :accountID1 and AccountID2 = :accountID2");
			$query->bindValue(':accountID1', $accountID['AccountID']);
			$query->bindValue(':accountID2', $accountID2);
			$query->execute();
			
			$conn->commit();
			
			jsonResponse(1, "Friend deleted.");
			
		}
		catch (Exception $e) 																																																																								
		{
			$conn->rollBack();
			jsonResponse(0, "Error.");
		}
	}
	else
	{
		jsonResponse(0, "Error.");
	}
}

function getInvitations($email, $password)
{
	if(authenticate($email, $password))
	{	
		global $conn;
		
		$query = $conn->prepare("SELECT AccountID FROM Accounts WHERE Email = :email");
		$query->bindValue(':email', $email);
		$query->execute();
		$accountID = $query->fetch();
		
		$query = $conn->prepare("SELECT * FROM Invitations WHERE AccountID = :accountID"); 
		$query->bindValue(":accountID",$accountID['AccountID']);
		$query->execute(); 		
		$response = array();
		$response['invitations'] = array();
		
		while($fetch = $query->fetch())
		{
			$invite['InviteID'] = $fetch['InviteID'];
			$invite['EventID'] = $fetch['EventID'];
			array_push($response['invitations'], $invite);
			
		}	
		echo json_encode($response);		
	}
	else
	{
		jsonResponse(0, "Error.");
	}	
}

function addInvitation($email, $password, $AccountID, $EventID)
{
	if(authenticate($email, $password))
	{	
		global $conn;
		
		$query = $conn->prepare("SELECT AccountID FROM Accounts WHERE Email = :email");
		$query->bindValue(':email', $email);
		$query->execute();
		$accountID = $query->fetch();
		
		try 
		{  	
			$conn->beginTransaction();
			
			$query = $conn->prepare("INSERT INTO Invitations(AccountID, EventID) 
									 VALUES(:accountID, :eventID)");
									 
			$query->bindValue(':accountID', $accountID['AccountID']);
			$query->bindValue(':eventID', $eventID);
			$query->execute();
			
			$conn->commit();
			
			jsonResponse(1, "Invitation added.");
			
		}
		catch (Exception $e) 																																																																								
		{
			$conn->rollBack();
			jsonResponse(0, "Error.");
		}
	}
	else
	{
		jsonResponse(0, "Error.");
	}
}

function deleteInvitation($email, $password, $inviteID)
{
	if(authenticate($email, $password))
	{	
		global $conn;
		
		try 
		{  	
			$conn->beginTransaction();
			
			$query = $conn->prepare("DELETE FROM Invitations WHERE InviteID = :inviteID");
			$query->bindValue(':inviteID', $inviteID);
			$query->execute();
			
			$conn->commit();
			
			jsonResponse(1, "Invitation deleted.");
			
		}
		catch (Exception $e) 																																																																								
		{
			$conn->rollBack();
			jsonResponse(0, "Error.");
		}
	}
	else
	{
		jsonResponse(0, "Error.");
	}
}

function createAccount($login, $password, $firstname, $lastname, $email, $mic, $pic, $status, $lang, $rep,
					   $PSN_Account, $XBOX_Account, $STEAM_Account, $ORIGIN_Account, $DISCORD_Account, $UPLAY_Account, $BATTLE_Account, $LOL_Account, $SKYPE_Account)
{
	
	if(!isEmailExist($email))
	{	
		try 
		{  
			global $conn;
			
			$conn->beginTransaction();
			
			$query = $conn->prepare("INSERT INTO Accounts(Login, Password, FirstName, LastName, Email, Microphone, ProfilePicture, Status, Language, RepPoint ) 
									 VALUES(:login, :password, :firstname, :lastname, :email, :mic, :pic, :status, :lang, :rep)");
									 
			$query->bindValue(':login', $login);
			$query->bindValue(':password', $password);
			$query->bindValue(':firstname', $firstname);
			$query->bindValue(':lastname', $lastname);
			$query->bindValue(':email', $email);
			$query->bindValue(':mic', $mic);
			$query->bindValue(':pic', $pic);
			$query->bindValue(':status', $status);
			$query->bindValue(':lang', $lang);
			$query->bindValue(':rep', $rep);
			$query->execute();
			
			$conn->commit();
		} 
		catch (Exception $e) 																																																																								
		{
			$conn->rollBack();
			jsonResponse(0, "Error.");
			return 0;
		}	
		try 
		{  
			$conn->beginTransaction();
			//getAccountByLogin($login);
			$query = $conn->prepare("INSERT INTO LinkAccount(PSN_Account, XBOX_Account, STEAM_Account, ORIGIN_Account, DISCORD_Account, UPLAY_Account, BATTLE_Account, LOL_Account, SKYPE_Account, AccountID ) 
									 VALUES(:PSN_Account, :XBOX_Account, :STEAM_Account, :ORIGIN_Account, :DISCORD_Account, :UPLAY_Account, :BATTLE_Account, :LOL_Account, :SKYPE_Account, :AccountID)");
									 
			$query->bindValue(':PSN_Account', $PSN_Account);
			$query->bindValue(':XBOX_Account', $XBOX_Account);
			$query->bindValue(':STEAM_Account', $STEAM_Account);
			$query->bindValue(':ORIGIN_Account', $ORIGIN_Account);
			$query->bindValue(':DISCORD_Account', $DISCORD_Account);
			$query->bindValue(':UPLAY_Account', $UPLAY_Account);
			$query->bindValue(':BATTLE_Account', $BATTLE_Account);
			$query->bindValue(':LOL_Account', $LOL_Account);
			$query->bindValue(':SKYPE_Account', $SKYPE_Account);
			$acc = getAccountByEmail($email, false);
			$query->bindValue(':AccountID', $acc['AccountID']);
			$query->execute();
			
			$conn->commit();
			
			//jsonResponse(1, "Profile created.");
			jsonResponse(1, "Profile created.");
	  
		} 
		catch (Exception $e) 																																																																								
		{
			$conn->rollBack();
			jsonResponse(0, "Error.");
			return 0;
		}		
				
	}
	else
	{
		jsonResponse(0, "Login exist.");	
	}
	
}

function updateAccount($email, $password, $column, $value)
{
	if(authenticate($email, $password) == true) 
	{	
		global $query;
		
		switch ((int)$column) 
		{
			case 0://Password
				$query = 'UPDATE Accounts SET Accounts.Password=:value WHERE Accounts.Email=:email';
				break;
			case 1://Last
				$query = 'UPDATE Accounts SET Accounts.LastName=:value WHERE Accounts.Email=:email';
				break;
			case 2:
				$query = 'UPDATE Accounts SET Accounts.FirstName=:value WHERE Accounts.Email=:email';
				break;
			case 3:
				$query = 'UPDATE Accounts SET Login=:value WHERE Email=:email';
				break;
			case 4:
				$query = 'UPDATE Accounts SET Accounts.Password=:value WHERE Accounts.Email=:email';
				break;
			case 5:
				$query = 'UPDATE Accounts SET Accounts.Description=:value WHERE Accounts.Email=:email';
				break;
			case 6:
				$query = 'UPDATE Accounts SET Accounts.Microphone=:value WHERE Accounts.Email=:email';
				break;
			case 7:
				$query = 'UPDATE Accounts SET Accounts.ProfilePicture=:value WHERE Accounts.Email=:email';
				break;
			case 8:
				$query = 'UPDATE Accounts SET Accounts.Status=:value WHERE Accounts.Email=:email';
				break;
			case 9:
				$query = 'UPDATE Accounts SET Accounts.Language=:value WHERE Accounts.Email=:email';
				break;
			case 10:
				$query = 'UPDATE Accounts SET Accounts.RepPoint=:value WHERE Accounts.Email=:email';
				break;
			case 11:
				$query = 'UPDATE LinkAccount SET LinkAccount.PSN_Account=:value WHERE LinkAccount.AccountID=:accountid';
				break;
			case 12:
				$query = 'UPDATE LinkAccount SET LinkAccount.XBOX_Account=:value WHERE LinkAccount.AccountID=:accountid';
				break;
			case 13:
				$query = 'UPDATE LinkAccount SET LinkAccount.STEAM_Account=:value WHERE LinkAccount.AccountID=:accountid';
				break;
			case 14:
				$query = 'UPDATE LinkAccount SET LinkAccount.ORIGIN_Account=:value WHERE LinkAccount.AccountID=:accountid';
				break;
			case 15:
				$query = 'UPDATE LinkAccount SET LinkAccount.DISCORD_Account=:value WHERE LinkAccount.AccountID=:accountid';
				break;
			case 16:
				$query = 'UPDATE LinkAccount SET LinkAccount.UPLAY_Account=:value WHERE LinkAccount.AccountID=:accountid';
				break;	
			case 17:
				$query = 'UPDATE LinkAccount SET LinkAccount.BATTLE_Account=:value WHERE LinkAccount.AccountID=:accountid';
				break;	
			case 18:
				$query = 'UPDATE LinkAccount SET LinkAccount.LOL_Account=:value WHERE LinkAccount.AccountID=:accountid';
				break;	
			case 19:
				$query = 'UPDATE LinkAccount SET LinkAccount.SKYPE_Account=:value WHERE LinkAccount.AccountID=:accountid';
				break;					
			default:
				jsonResponse(0, "Error acc.");
				return;
				break;
			
		}
		try 
		{  
			global $conn;
		
			$conn->beginTransaction();
			$q = $conn->prepare($query);
			$q->bindValue(':value', $value);
			
			if((int)$column > 10)
			{
				$acc = getAccountByEmail($email, false);
				$q->bindValue(':accountid',(int)$acc['AccountID']);
			}
			else
			{
				$q->bindValue(':email', $email);
			}
			$q->execute();
			$conn->commit();
			
			jsonResponse(1, 'Profile edited.');
	  
		} 
		catch (Exception $e) 																																																																								
		{
			$conn->rollBack();
			jsonResponse(0, $e);
			return 0;
		}
	}
}


function getAccountByEmail($email, $json = true)
{
	if(isEmailExist($email))
	{
			global $conn;
			
			$query = $conn->prepare("SELECT AccountID, Login, FirstName, LastName, Email, Microphone, ProfilePicture, Status, Language, RepPoint, Description FROM Accounts WHERE Email = :email");
			$query->bindValue(':email', $email);
			$query->execute();
			$fetch = $query->fetch();
			$response = array();
			$response['AccountID'] = $fetch['AccountID'];
			$response['Login'] = $fetch['Login'];
			$response['FirstName'] = $fetch['FirstName'];
			$response['LastName'] = $fetch['LastName'];
			$response['Email'] = $fetch['Email'];
			$response['Description'] = $fetch['Description'];
			$response['Microphone'] = $fetch['Microphone'];		
			$response['ProfilePicture'] = $fetch['ProfilePicture'];		
			$response['Status'] = $fetch['Status'];		
			$response['Language'] = $fetch['Language'];		
			$response['RepPoint'] = $fetch['RepPoint'];		
			$response['success'] = 1;		
			
			if($json)
				echo json_encode($response);
			
			return $response;
	}
	else
	{
		
		jsonResponse(0,"Account not exist.");
	}
	
}

function getLinkAccounts($email, $password)
{
	if(authenticate($email, $password) == true) 
	{
			global $conn;
			
			$query = $conn->prepare("SELECT LinkAccountID, LinkAccount.AccountID, PSN_Account, XBOX_Account, STEAM_Account, ORIGIN_Account, DISCORD_Account, UPLAY_Account, BATTLE_Account, LOL_Account, SKYPE_Account FROM Accounts, LinkAccount WHERE LinkAccount.AccountID = Accounts.AccountID and Email = :email");
			$query->bindValue(':email', $email);
			$query->execute();
			$fetch = $query->fetch();
			$response = array();
			$response['AccountID'] = $fetch['AccountID'];
			$response['PSN_Account'] = $fetch['PSN_Account'];
			$response['XBOX_Account'] = $fetch['XBOX_Account'];
			$response['STEAM_Account'] = $fetch['STEAM_Account'];
			$response['ORIGIN_Account'] = $fetch['ORIGIN_Account'];
			$response['DISCORD_Account'] = $fetch['DISCORD_Account'];
			$response['UPLAY_Account'] = $fetch['UPLAY_Account'];		
			$response['BATTLE_Account'] = $fetch['BATTLE_Account'];		
			$response['LOL_Account'] = $fetch['LOL_Account'];		
			$response['SKYPE_Account'] = $fetch['SKYPE_Account'];		
			$response['LinkAccountID'] = $fetch['LinkAccountID'];		
			$response['success'] = 1;		

			echo json_encode($response);
	}
	else
	{
		
		jsonResponse(0,"Account not exist.");
	}
	
}

function login($email, $password)
{
	if(authenticate($email, $password) == true) 
	{
		jsonResponse(1,"Logged");
	}
	else	
	{
		jsonResponse(0,"Invalid");	
	}
}

function isAccountExist($email)
{
	if(isEmailExist($email) == true) 
	{
		jsonResponse(1,"Exist");
	}
	else	
	{
		jsonResponse(0,"Not");	 
	}
}

function getHashByEmail($email)
{
	$response = array();
	$hash = loadHashByEmail($email);
	if(strlen($hash) > 5)
	{
		$response['success'] = 1;
		$response['Password'] = $hash;
		echo json_encode($response);
	}
	else
	{
		$response['success'] = 0;
		echo json_encode($response);		
	}

}
function safe_json_encode($value){
    if (version_compare(PHP_VERSION, '5.4.0') >= 0) {
        $encoded = json_encode($value, JSON_PRETTY_PRINT);
    } else {
        $encoded = json_encode($value);
    }
    switch (json_last_error()) {
        case JSON_ERROR_NONE:
            return $encoded;
        case JSON_ERROR_DEPTH:
            return 'Maximum stack depth exceeded'; // or trigger_error() or throw new Exception()
        case JSON_ERROR_STATE_MISMATCH:
            return 'Underflow or the modes mismatch'; // or trigger_error() or throw new Exception()
        case JSON_ERROR_CTRL_CHAR:
            return 'Unexpected control character found';
        case JSON_ERROR_SYNTAX:
            return 'Syntax error, malformed JSON'; // or trigger_error() or throw new Exception()
        case JSON_ERROR_UTF8:
            $clean = utf8ize($value);
            return safe_json_encode($clean);
        default:
            return 'Unknown error'; // or trigger_error() or throw new Exception()

    }
}

function utf8ize($mixed) {
    if (is_array($mixed)) {
        foreach ($mixed as $key => $value) {
            $mixed[$key] = utf8ize($value);
        }
    } else if (is_string ($mixed)) {
        return utf8_encode($mixed);
    }
    return $mixed;
}
?>