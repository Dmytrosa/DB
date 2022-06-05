SELECT DISTINCT A.Name
FROM Herbalists AS A
WHERE NOT EXISTS(
	(SELECT H_Ps.PoisonerId
	FROM H_Ps INNER JOIN Herbalists
	ON H_Ps.HerbalistId = Herbalists.Id
	WHERE(Herbalists.Name = HerbalistName)
	)EXCEPT(
	SELECT FA.PoisonerId
	FROM H_Ps AS FA
	WHERE(FA.HerbalistId = A.Id)
	)
);