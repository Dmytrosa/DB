SELECT DISTINCT Poisoners.Name
FROM Poisoners
WHERE Poisoners.Id IN
	(SELECT H_Ps.PoisonerId
	 FROM H_Ps INNER JOIN Herbalists
	 ON Herbalists.Id = H_Ps.HerbalistId
	 WHERE Herbalists.Name = HerbalistName);