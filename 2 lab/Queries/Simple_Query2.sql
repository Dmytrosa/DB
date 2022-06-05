SELECT Poisoners.Name
FROM Poisoners
WHERE Poisoners.Id IN
	(SELECT P_Ps.PoisonerId
	 FROM P_Ps INNER JOIN Poisons
	 ON Poisons.Id = P_Ps.PoisonId
	 WHERE Poisons.Name = PoisonName AND Poisoners.BirthDate > PoisonerBirthDate);