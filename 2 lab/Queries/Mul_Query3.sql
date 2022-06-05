SELECT G.Name
FROM Poisons AS G
WHERE NOT EXISTS(
	(SELECT FG.PoisonerId
	FROM P_Ps AS FG INNER JOIN Poisoners
	ON FG.PoisonerId = Poisoners.Id
	WHERE Poisoners.BirthDate = PoisonerBirthDate
	)EXCEPT(
	SELECT FG2.PoisonerId
	FROM P_Ps AS FG2
	WHERE FG2.PoisonId = G.Id
	)
);