DECLARE @P nvarchar(50) = PoisonerName;

SELECT P.Name
FROM Poisoners P
WHERE P.Name != @P
AND NOT EXISTS
((SELECT P_Ps.PoisonId
  FROM P_Ps
  WHERE P_Ps.PoisonerId = P.Id)
 EXCEPT
 (SELECT P_Ps.PoisonId
  FROM P_Ps
	INNER JOIN Poisoners ON P_Ps.PoisonerId = Poisoners.Id
  WHERE Poisoners.Name = @P))
AND NOT EXISTS
((SELECT P_Ps.PoisonId
  FROM P_Ps
	INNER JOIN Poisoners ON P_Ps.PoisonerId = Poisoners.Id
  WHERE Poisoners.Name = @P)
 EXCEPT
(SELECT P_Ps.PoisonId
  FROM P_Ps
  WHERE P_Ps.PoisonerId = P.Id));