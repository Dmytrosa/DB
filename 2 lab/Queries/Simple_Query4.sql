SELECT DISTINCT Poisoners.Name, Poisoners.BirthDate
FROM Poisoners
WHERE Poisoners.Id NOT IN
	(SELECT P_Ps.PoisonerId
	 FROM P_Ps INNER JOIN Poisons
	 ON Poisons.Id = P_Ps.PoisonId
	 WHERE Poisons.Name = PoisonName);