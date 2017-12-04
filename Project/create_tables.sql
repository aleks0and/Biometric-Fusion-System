drop table FaceBiometric
drop table VoiceBiometric

create table FaceBiometric
(
Id integer foreign key references Person(Id),
FeatureVector varbinary(max)
)

create table VoiceBiometric
(
Id integer foreign key references Person(Id),
FeatureVector varbinary(max)
)