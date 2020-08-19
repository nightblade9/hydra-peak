// Matches Models.User in C#

export interface IDungeon
{
    id: any, // from mongoDB
    currentFloor: IFloor
};

export interface IFloor
{
    id: any,
    floorNumber: number
}