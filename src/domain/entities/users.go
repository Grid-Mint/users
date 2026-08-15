package entities

import (
	"lacarte/users/domain/enums"
	"time"
)

type User struct {
	ID           string
	Email        string
	FullName     string
	FirstName    string
	LastName     string
	PasswordHash string
	CreatedAt    time.Time
	UpdatedAt    time.Time
	Role         enums.Role
	Status    	 enums.Status
}
