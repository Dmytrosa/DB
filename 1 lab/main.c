#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include <ctype.h>

#define MaxStringSize 50
#define MaxRecords 20


struct MetaData
{
    int maxRecords;
    int firstData;
    int physicallySpace;
    int autoNum;
} metadataMaster, metadataSlave;


struct Poisoner
{
    int id;
    int firstSlave;
    int slaveCounter;
    char title[MaxStringSize];
    int teamSize;
};

struct PoisonerInd
{
    int id;
    int address;
    bool isDel;
};

struct Recipe
{
    int id;
    int id_rs;
    int id_rm;
    bool isDel;
    int nextSlave;
    char title[MaxStringSize];
    char igredients[MaxStringSize];
};

struct Buffer
{
    char buf[256][256];
    int argcounter;
} buffer;




void init_master()
{
    FILE* P_ind, * P_fl;

    P_ind = fopen("P.ind", "a+b");

    if (fseek(P_ind, 0, SEEK_END) == 0 && ftell(P_ind) == 0)
    {
        metadataMaster.maxRecords = MaxRecords;
        metadataMaster.firstData = sizeof(metadataMaster);
        metadataMaster.physicallySpace = 0;
        metadataMaster.autoNum = 0;

        fwrite(&metadataMaster, sizeof(metadataMaster), 1, P_ind);
        printf("P.ind init: DONE\n");

        P_fl = fopen("P.fl", "wb");
        fclose(P_fl);
    }
    else
    {
        fseek(P_ind, 0, SEEK_SET);
        fread(&metadataMaster, sizeof(metadataMaster), 1, P_ind);
    }

    // struct MetaData tmp;
    // fseek(P_ind, 0, SEEK_SET);
    // fread(&tmp, sizeof(tmp), 1, P_ind);
    // printf("maxRecords: %d\nFirstRecord: %d\nData: %d, %d, %d", tmp.maxRecords, tmp.firstData,
    // tmp.markedData[0], tmp.markedData[1], tmp.markedData[2]);
    fclose(P_ind);
}

void init_slave()
{
    FILE* R_fl;
    R_fl = fopen("R.fl", "a+b");
    if (fseek(R_fl, 0, SEEK_END) == 0 && ftell(R_fl) == 0)
    {
        metadataSlave.maxRecords = MaxRecords;
        metadataSlave.firstData = sizeof(metadataSlave);
        metadataSlave.physicallySpace = 0;
        metadataSlave.autoNum = 0;

        fwrite(&metadataSlave, sizeof(metadataSlave), 1, R_fl);
        printf("R.fl init: DONE\n");
    }
    else
    {
        fseek(R_fl, 0, SEEK_SET);
        fread(&metadataSlave, sizeof(metadataSlave), 1, R_fl);
    }

    fclose(R_fl);
}

void init()
{
    init_master();
    init_slave();
}

void input()
{
    char tmp[256];

    buffer.argcounter = 0;
    gets(tmp);

    char* token = strtok(tmp, " ");
    while (token)
    {
        strcpy(buffer.buf[buffer.argcounter], token);
        ++buffer.argcounter;
        token = strtok(NULL, " ");
    }
}


bool checkMinArgCounter(int n)
{
    if (buffer.argcounter < n)
    {
        printf("Invalid number of arguments!\n");
        return false;
    }
    else return true;
}

bool isnumber(const char* string)
{
    for (int i = 0; string[i] != '\0'; ++i)
    {
        if (isdigit(string[i]) == 0 && string[i] != '-') return false;
    }
    return true;
}

void save()
{
    FILE* P_ind = fopen("P.ind", "r+b");
    FILE* R_fl = fopen("R.fl", "r+b");

    rewind(P_ind);
    rewind(R_fl);

    fwrite(&metadataMaster, sizeof(metadataMaster), 1, P_ind);
    fwrite(&metadataSlave, sizeof(metadataSlave), 1, R_fl);

    fclose(P_ind);
    fclose(R_fl);
}

void get_m()
{
    if (!checkMinArgCounter(2)) return;

    if (!isnumber(buffer.buf[1]))
    {
        printf("Second argument must be a number\n");
        return;
    }

    FILE* P_ind;
    FILE* P_fl;
    struct Poisoner gmstudio;
    struct PoisonerInd gmstudioind;
    int posFl = -1;
    int id = atoi(buffer.buf[1]);
    P_ind = fopen("P.ind", "rb");
    P_fl = fopen("P.fl", "rb");

    fseek(P_ind, metadataMaster.firstData, SEEK_SET);
    for (int i = 0; i < metadataMaster.physicallySpace; ++i)
    {
        fread(&gmstudioind, sizeof(gmstudioind), 1, P_ind);
        if (gmstudioind.id == id)
        {
            posFl = gmstudioind.address;
            break;
        }
    }


    if (posFl == -1)
    {
        printf("Data with given id doesn't exist\n");
        return;
    }
    else
    {
        fseek(P_fl, posFl, SEEK_SET);
        fread(&gmstudio, sizeof(gmstudio), 1, P_fl);
        printf("Data: id: %d, title: %s, team size: %d\n", gmstudio.id, gmstudio.title, gmstudio.teamSize);
    }

    fclose(P_fl);
    fclose(P_ind);
}

void get_s()
{
    if (!checkMinArgCounter(2)) return;
    if (!isnumber(buffer.buf[1]))
    {
        printf("First argument must be a number\n");
        return;
    }

    if (!isnumber(buffer.buf[2]))
    {
        printf("First argument must be a number\n");
        return;
    }


    FILE* P_ind = fopen("P.ind", "rb");
    FILE* P_fl = fopen("P.fl", "r+b");
    FILE* R_fl = fopen("R.fl", "r+b");
    struct Poisoner gmstudio;
    struct PoisonerInd gmstudioind;
    struct Recipe gm;
    int posP_fl = -1;
    int id_rs = atoi(buffer.buf[1]);
    int id_rm = atoi(buffer.buf[2]);

    // get-m
    fseek(P_ind, metadataMaster.firstData, SEEK_SET);
    for (int i = 0; i < metadataMaster.physicallySpace; ++i)
    {
        fread(&gmstudioind, sizeof(gmstudioind), 1, P_ind);
        if (gmstudioind.id == id_rs)
        {
            posP_fl = gmstudioind.address;
            break;
        }
    }

    if (posP_fl == -1)
    {
        printf("Recipe Studio with given id doesn't exist\n");
        return;
    }

    fseek(P_fl, posP_fl, SEEK_SET);
    fread(&gmstudio, sizeof(gmstudio), 1, P_fl);
    //

    // Finding data with given id_rm
    gm.nextSlave = gmstudio.firstSlave;
    for (int i = 0; i < gmstudio.slaveCounter; ++i)
    {
        fseek(R_fl, gm.nextSlave, SEEK_SET);
        fread(&gm, sizeof(struct Recipe), 1, R_fl);
        if (gm.id_rs == id_rm && gm.isDel == false) break;
    }
    //

    if (gm.id_rm == id_rm && gm.id_rs == id_rs)
    {
        printf("Data: id: %d, id_rs: %d, id_rm: %d, title: %s, Ingredients: %s\n",
            gm.id, gm.id_rs, gm.id_rm, gm.title, gm.igredients);
    }
    else
    {
        printf("Recipe with given id doesn't exist\n");
    }

    fclose(P_fl);
    fclose(P_ind);
    fclose(R_fl);
}

void del_m()
{
    if (!checkMinArgCounter(2)) return;
    if (!isnumber(buffer.buf[1]))
    {
        printf("First argument must be a number\n");
        return;
    }

    FILE* P_ind = fopen("P.ind", "r+b");
    FILE* P_fl = fopen("P.fl", "r+b");
    FILE* R_fl = fopen("R.fl", "r+b");
    struct PoisonerInd gmstudioind;
    struct Poisoner gmstudio;
    struct Recipe gm;
    bool del = false;
    int id = atoi(buffer.buf[1]);

    fseek(P_ind, metadataMaster.firstData, SEEK_SET);
    for (int i = 0; i < metadataMaster.physicallySpace; ++i)
    {
        fread(&gmstudioind, sizeof(gmstudioind), 1, P_ind);
        if (del == true)
        {
            if (fseek(P_ind, -2 * sizeof(struct PoisonerInd), SEEK_CUR) == 0)
            {
                fseek(P_fl, gmstudioind.address, SEEK_SET);
                fread(&gmstudio, sizeof(struct Poisoner), 1, P_fl);
                fseek(P_fl, -2 * sizeof(struct Poisoner), SEEK_CUR);
                fwrite(&gmstudio, sizeof(struct Poisoner), 1, P_fl);
                fseek(P_fl, sizeof(struct Poisoner), SEEK_CUR);

                gmstudioind.address -= sizeof(struct Poisoner);
                fwrite(&gmstudioind, sizeof(struct PoisonerInd), 1, P_ind);
                fseek(P_ind, sizeof(struct PoisonerInd), SEEK_CUR);
            }
        }
        if (gmstudioind.id == id)
        {
            del = true;
            fread(&metadataSlave, sizeof(struct MetaData), 1, R_fl);
            fseek(P_fl, gmstudioind.address, SEEK_SET);
            fread(&gmstudio, sizeof(struct Poisoner), 1, P_fl);
            fseek(R_fl, gmstudio.firstSlave, SEEK_SET);

            for (int j = 0; j < gmstudio.slaveCounter; ++j)
            {
                fread(&gm, sizeof(struct Recipe), 1, R_fl);
                gm.isDel = true;
                fseek(R_fl, -sizeof(struct Recipe), SEEK_CUR);
                fwrite(&gm, sizeof(struct Recipe), 1, R_fl);
                fseek(R_fl, gm.nextSlave, SEEK_SET);
            }
        }
    }


    if (del == false)
    {
        printf("Data with given id doesn't exist\n");
        return;
    }
    --metadataMaster.physicallySpace;

    gmstudioind.address += sizeof(struct Poisoner);
    gmstudioind.isDel = true;
    fwrite(&gmstudioind, sizeof(struct PoisonerInd), 1, P_ind);
    fseek(P_ind, -sizeof(struct PoisonerInd), SEEK_CUR);
    fread(&gmstudioind, sizeof(struct PoisonerInd), 1, P_ind);


    printf("Data deleted!\n%d, %d, %d\n\n", gmstudioind.id, gmstudioind.address, gmstudioind.isDel);

    fclose(P_ind);
    fclose(P_fl);
    fclose(R_fl);

    save();
}

void del_s()
{
    if (!checkMinArgCounter(3)) return;
    if (!isnumber(buffer.buf[1]))
    {
        printf("First argument must be a number\n");
        return;
    }

    FILE* P_ind = fopen("P.ind", "r+b");
    FILE* P_fl = fopen("P.fl", "r+b");
    FILE* R_fl = fopen("R.fl", "r+b");
    struct PoisonerInd gmstudioind;
    struct Poisoner gmstudio;
    gmstudio.id = -1;
    struct Recipe gm;
    bool del = false;
    int gs_id = atoi(buffer.buf[1]);
    int g_id = atoi(buffer.buf[2]);

    fseek(P_ind, metadataMaster.firstData, SEEK_SET);
    for (int i = 0; i < metadataMaster.physicallySpace; ++i)
    {
        fread(&gmstudioind, sizeof(gmstudioind), 1, P_ind);
        if (gmstudioind.isDel == false && gmstudioind.id == gs_id)
        {
            fread(&gmstudio, sizeof(struct Poisoner), 1, P_fl);
            break;
        }
    }


    if (gmstudio.id == -1)
    {
        printf("Recipe Studio with given id doesn't exist\n");
        return;
    }


    fseek(R_fl, gmstudio.firstSlave, SEEK_SET);
    for (int i = 0; i < metadataSlave.physicallySpace; ++i)
    {
        fread(&gm, sizeof(struct Recipe), 1, R_fl);
        if (gm.id_rm == g_id && gm.isDel == false)
        {

            gm.isDel = true;
            fseek(R_fl, -sizeof(struct Recipe), SEEK_CUR);
            fwrite(&gm, sizeof(struct Recipe), 1, R_fl);
            break;
        }
    }


    if (gm.id_rm != g_id)
    {
        printf("Recipe with given id doesn't exist\n");
    }
    else
    {
        printf("Data deleted!\n");
    }

    fclose(P_ind);
    fclose(P_fl);
    fclose(R_fl);

    save();
}

void update_m()
{

    if (!checkMinArgCounter(3)) return;
    if (!isnumber(buffer.buf[1]))
    {
        printf("First argument must be a number\n");
        return;
    }
    if (!isnumber(buffer.buf[3]))
    {
        printf("Third argument must be a number\n");
        return;
    }

    FILE* P_ind = fopen("P.ind", "r+b");
    FILE* P_fl = fopen("P.fl", "r+b");
    struct PoisonerInd gmstudioind;
    struct Poisoner gmstudio;
    int id = atoi(buffer.buf[1]);
    char* title = buffer.buf[2];
    int teamSize = atoi(buffer.buf[3]);

    fseek(P_ind, metadataMaster.firstData, SEEK_SET);
    for (int i = 0; i < metadataMaster.physicallySpace; ++i)
    {
        fread(&gmstudioind, sizeof(gmstudioind), 1, P_ind);
        if (gmstudioind.id == id && gmstudioind.isDel == false)
        {
            fseek(P_ind, gmstudioind.address, SEEK_SET);
            fread(&gmstudio, sizeof(struct Poisoner), 1, P_fl);
            fseek(P_fl, -sizeof(struct Poisoner), SEEK_CUR);

            strcpy(gmstudio.title, title);
            gmstudio.teamSize = teamSize;

            fwrite(&gmstudio, sizeof(struct Poisoner), 1, P_fl);
        }
    }


    if (gmstudioind.id != id)
    {
        printf("Recipe Studio with given id doesn't exist!\n");
    }
    else
    {
        printf("Data updated!\n");
    }
    fclose(P_ind);
    fclose(P_fl);
}

void update_s()
{

    if (!checkMinArgCounter(4)) return;
    if (!isnumber(buffer.buf[1]))
    {
        printf("First argument must be a number\n");
        return;
    }
    if (!isnumber(buffer.buf[2]))
    {
        printf("Second argument must be a number\n");
        return;
    }

    FILE* P_ind = fopen("P.ind", "r+b");
    FILE* P_fl = fopen("P.fl", "r+b");
    FILE* R_fl = fopen("R.fl", "r+b");
    struct PoisonerInd gmstudioind;
    struct Poisoner gmstudio;
    gmstudio.id = -1;
    struct Recipe gm;
    bool del = false;
    int gs_id = atoi(buffer.buf[1]);
    int g_id = atoi(buffer.buf[2]);
    char* title = buffer.buf[3];
    char* igredients = buffer.buf[4];

    fseek(P_ind, metadataMaster.firstData, SEEK_SET);
    for (int i = 0; i < metadataMaster.physicallySpace; ++i)
    {
        fread(&gmstudioind, sizeof(gmstudioind), 1, P_ind);
        if (gmstudioind.isDel == false && gmstudioind.id == gs_id)
        {
            fread(&gmstudio, sizeof(struct Poisoner), 1, P_fl);
            break;
        }
    }


    if (gmstudio.id == -1)
    {
        printf("Recipe Studio with given id doesn't exist\n");
        return;
    }


    fseek(R_fl, gmstudio.firstSlave, SEEK_SET);
    for (int i = 0; i < metadataSlave.physicallySpace; ++i)
    {
        fread(&gm, sizeof(struct Recipe), 1, R_fl);
        if (gm.id_rm == g_id && gm.isDel == false)
        {

            strcpy(gm.title, title);
            strcpy(gm.igredients, igredients);
            fseek(R_fl, -sizeof(struct Recipe), SEEK_CUR);
            fwrite(&gm, sizeof(struct Recipe), 1, R_fl);
            break;
        }
    }


    if (gm.id_rm != g_id)
    {
        printf("Recipe with given id doesn't exist\n");
    }
    else
    {
        printf("Data updated!\n");
    }

    fclose(P_ind);
    fclose(P_fl);
    fclose(R_fl);

}

void insert_m()
{
    if (!checkMinArgCounter(3)) return;
    if (!isnumber(buffer.buf[2]))
    {
        printf("Second argument must be a number\n");
        return;
    }


    FILE* P_ind = fopen("P.ind", "r+b");
    FILE* P_fl = fopen("P.fl", "r+b");
    struct Poisoner gmstudio;
    struct PoisonerInd gmstudioind;
    int posInd;
    int posFl;


    posInd = metadataMaster.firstData;
    fseek(P_ind, posInd, SEEK_SET);
    for (int i = 0; i < metadataMaster.physicallySpace; ++i)
    {
        fread(&gmstudioind, sizeof(gmstudioind), 1, P_ind);
        if (gmstudioind.isDel == true)
        {
            posInd = ftell(P_ind);
            posFl = gmstudioind.address;
            break;
        }
    }


    if (gmstudioind.isDel == false)
    {
        if (fread(&gmstudioind, sizeof(struct PoisonerInd), 1, P_ind) == 1)
        {
            posInd = ftell(P_ind) - sizeof(struct PoisonerInd);
            posFl = gmstudioind.address;
        }
        else
        {
            fseek(P_ind, 0, SEEK_END);
            fseek(P_fl, 0, SEEK_END);
            posInd = ftell(P_ind);
            posFl = ftell(P_fl);
        }
    }

    fseek(P_ind, posInd, SEEK_SET);
    fseek(P_fl, posFl, SEEK_SET);

    gmstudio.firstSlave = 0;
    gmstudio.id = metadataMaster.autoNum;
    strcpy(gmstudio.title, buffer.buf[1]);
    gmstudio.slaveCounter = 0;
    gmstudio.teamSize = atoi(buffer.buf[2]);

    gmstudioind.id = metadataMaster.autoNum++;
    gmstudioind.isDel = false;
    gmstudioind.address = ftell(P_fl);

    ++metadataMaster.physicallySpace;


    fwrite(&gmstudio, sizeof(gmstudio), 1, P_fl);
    fwrite(&gmstudioind, sizeof(gmstudioind), 1, P_ind);

    printf("Inserted: id: %d, isDel: %d\n", gmstudioind.id, gmstudioind.isDel);

    fclose(P_fl);
    fclose(P_ind);

    save();
}

// master_id, slave_id, 
void insert_s()
{
    if (!checkMinArgCounter(4)) return;
    if (!isnumber(buffer.buf[1]))
    {
        printf("First argument must be a number\n");
        return;
    }
    if (!isnumber(buffer.buf[2]))
    {
        printf("Second argument must be a number\n");
        return;
    }


    FILE* P_ind = fopen("P.ind", "rb");
    FILE* P_fl = fopen("P.fl", "r+b");
    FILE* R_fl = fopen("R.fl", "r+b");
    struct Poisoner gmstudio;
    struct PoisonerInd gmstudioind;
    struct Recipe gm;
    int posP_fl = -1;
    int posR_fl = -1;
    int id_rs = atoi(buffer.buf[1]);
    int id_rm = atoi(buffer.buf[2]);

    // get-m
    fseek(P_ind, metadataMaster.firstData, SEEK_SET);
    for (int i = 0; i < metadataMaster.physicallySpace; ++i)
    {
        fread(&gmstudioind, sizeof(gmstudioind), 1, P_ind);
        if (gmstudioind.id == id_rs)
        {
            posP_fl = gmstudioind.address;
            break;
        }
    }

    if (posP_fl == -1)
    {
        printf("Recipe Studio with given id doesn't exist\n");
        return;
    }

    fseek(P_fl, posP_fl, SEEK_SET);
    fread(&gmstudio, sizeof(gmstudio), 1, P_fl);
    //

    // Finding position for inserting in R.fl
    fread(&metadataSlave, sizeof(struct MetaData), 1, R_fl);

    for (int i = 0; i < metadataSlave.physicallySpace; ++i)
    {
        fread(&gm, sizeof(struct Recipe), 1, R_fl);
        if (gm.isDel == true)
        {
            posR_fl = sizeof(struct MetaData) + (i) * sizeof(struct Recipe);
            break;
        }
    }

    if (posR_fl == -1)
    {
        posR_fl = ftell(R_fl);
        ++metadataSlave.physicallySpace;
    }
    //

    // Finding last data with this id_m
    gm.nextSlave = gmstudio.firstSlave;
    for (int i = 0; i < gmstudio.slaveCounter; ++i)
    {
        fseek(R_fl, gm.nextSlave, SEEK_SET);
        fread(&gm, sizeof(struct Recipe), 1, R_fl);
    }
    //

    // Updating gm.nextSlave
    if (gmstudio.slaveCounter != 0)
    {
        gm.nextSlave = posR_fl;
        fseek(R_fl, -sizeof(struct Recipe), SEEK_CUR);
        fwrite(&gm, sizeof(struct Recipe), 1, R_fl);
    }
    else
    {
        gmstudio.firstSlave = posR_fl;
    }
    //

    // Inserting gm
    gm.id = metadataSlave.autoNum++;
    gm.isDel = false;
    gm.id_rs = id_rs;
    gm.id_rm = id_rm;
    strcpy(gm.title, buffer.buf[3]);
    strcpy(gm.igredients, buffer.buf[4]);
    gm.nextSlave = -1;

    fseek(R_fl, posR_fl, SEEK_SET);
    fwrite(&gm, sizeof(struct Recipe), 1, R_fl);
    //


    // Updating gmstuido.slaveCounter and metadataSlave.physicallySpace
    ++gmstudio.slaveCounter;

    fseek(P_fl, posP_fl, SEEK_SET);
    fwrite(&gmstudio, sizeof(struct Poisoner), 1, P_fl);
    //

    printf("Inserted: id: %d, id_rm: %d, id_rs: %d, title: %s, ingridients: %s\n",
        gm.id, gm.id_rm, gm.id_rs, gm.title, gm.igredients);

    fclose(P_fl);
    fclose(P_ind);
    fclose(R_fl);

    save();
}

void show()
{
    FILE* P_ind = fopen("P.ind", "r+b");
    FILE* P_fl = fopen("P.fl", "r+b");
    FILE* R_fl = fopen("R.fl", "r+b");

    struct PoisonerInd gmstudioind;
    struct Poisoner gmstudio;
    struct Recipe gm;

    fread(&metadataMaster, sizeof(metadataMaster), 1, P_ind);
    fread(&metadataSlave, sizeof(metadataSlave), 1, R_fl);


    printf("MetaData:\n");
    printf("\tMaster. autonum: %d, physicallySpace: %d, firstData: %d, maxRecords: %d\n",
        metadataMaster.autoNum, metadataMaster.physicallySpace, metadataMaster.firstData, metadataMaster.maxRecords);

    printf("\tSlave. autonum: %d, physicallySpace: %d, firstData: %d, maxRecords: %d\n",
        metadataSlave.autoNum, metadataSlave.physicallySpace, metadataSlave.firstData, metadataSlave.maxRecords);


    printf("\nP.ind:\n");
    for (int i = 0; i < metadataMaster.physicallySpace; ++i)
    {
        fread(&gmstudioind, sizeof(gmstudioind), 1, P_ind);
        if (gmstudioind.isDel == false)
        {
            printf("Id: %d, isDel: %d, address: %d\n", gmstudioind.id, gmstudioind.isDel, gmstudioind.address);
        }
    }

    printf("\nP.fl:\n");
    for (int i = 0; i < metadataMaster.physicallySpace; ++i)
    {
        fread(&gmstudio, sizeof(gmstudio), 1, P_fl);
        printf("Id: %d, firstSlave: %d, slaveCounter: %d, title: %s, teamSize: %d\n",
            gmstudio.id, gmstudio.firstSlave, gmstudio.slaveCounter, gmstudio.title, gmstudio.teamSize);
    }

    printf("\nR.fl:\n");
    for (int i = 0; i < metadataSlave.physicallySpace; ++i)
    {
        fread(&gm, sizeof(gm), 1, R_fl);
        if (gm.isDel == false)
            printf("Id: %d, id_rs: %d, id_rm: %d, isDel: %d, nextSlave: %d, title: %s, igredients: %s\n",
                gm.id, gm.id_rs, gm.id_rm, gm.isDel, gm.nextSlave, gm.title, gm.igredients);
    }



    fclose(P_ind);
    fclose(P_fl);
    fclose(R_fl);
}

void info()
{
    FILE* P_ind = fopen("P.ind", "r+b");
    FILE* P_fl = fopen("P.fl", "r+b");
    FILE* R_fl = fopen("R.fl", "r+b");

    struct PoisonerInd gmstudioind;
    struct Poisoner gmstudio;
    struct Recipe gm;
    int P_count = 0;
    int G_count = 0;

    fread(&metadataMaster, sizeof(metadataMaster), 1, P_ind);
    fread(&metadataSlave, sizeof(metadataSlave), 1, R_fl);


    for (int i = 0; i < metadataMaster.physicallySpace; ++i)
    {
        fread(&gmstudioind, sizeof(struct PoisonerInd), 1, P_ind);

        fseek(P_fl, gmstudioind.address, SEEK_SET);
        fread(&gmstudio, sizeof(struct Poisoner), 1, P_fl);
        P_count++;
        printf("Id: %d\n\tTotal amount of sub records: %d\n", gmstudio.id, gmstudio.slaveCounter - 1);
    }

    for (int i = 0; i < metadataSlave.physicallySpace; ++i)
    {
        fread(&gm, sizeof(struct Recipe), 1, R_fl);
        if (gm.isDel == false) G_count++;
    }

    printf("Total amount of records: %d\nTotal amount of sub records: %d\n", P_count, G_count);

    fclose(P_ind);
    fclose(P_fl);
    fclose(R_fl);
}

void process()
{
    if (strcmp(buffer.buf[0], "get-m") == 0)
    {
        get_m();
    }
    else if (strcmp(buffer.buf[0], "get-s") == 0)
    {
        get_s();
    }
    else if (strcmp(buffer.buf[0], "del-m") == 0)
    {
        del_m();
    }
    else if (strcmp(buffer.buf[0], "del-s") == 0)
    {
        del_s();
    }
    else if (strcmp(buffer.buf[0], "update-m") == 0)
    {
        update_m();
    }
    else if (strcmp(buffer.buf[0], "update-s") == 0)
    {
        update_s();
    }
    else if (strcmp(buffer.buf[0], "insert-m") == 0)
    {
        insert_m();
    }
    else if (strcmp(buffer.buf[0], "insert-s") == 0)
    {
        insert_s();
    }
    else if (strcmp(buffer.buf[0], "info") == 0)
    {
        info();
    }
    else if (strcmp(buffer.buf[0], "exit") == 0)
    {
        save();
        system("rm *.ind *.fl");
        exit(0);
    }
    else if (strcmp(buffer.buf[0], "print") == 0)
    {
        show();
    }

    buffer.buf[0][0] = '\0';
}

int main()
{
    init();
    while (true)
    {
        input();
        process();

        // break;
    }

    return 0;
}