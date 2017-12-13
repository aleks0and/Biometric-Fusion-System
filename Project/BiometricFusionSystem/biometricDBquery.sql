drop table Person
drop table FaceBiometric
drop table VoiceBiometric

create table Person
(
	Id integer identity (1,1) primary key,
	FirstName varchar(50),
	LastName varchar(50)
)

create table FaceBiometric
(
	Id integer foreign key references Person(Id),
	FeatureVector varchar(max)
)

create table VoiceBiometric
(
	Id integer foreign key references Person(Id),
	FeatureVector varchar(max),
	RecordedWord varchar(50)
)